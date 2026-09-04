import io
import re
from typing import Any

import spacy
import pdfplumber
from fastapi import Depends, FastAPI, File, HTTPException, UploadFile
from sqlalchemy.orm import Session
from sqlalchemy.dialects.postgresql import insert
from database import get_db
from matcher import calculate_match
from models import Applicant, ApplicantSkill, Job, JobMatch, JobSkill, Skill
nlp = spacy.load("en_core_web_sm")


app = FastAPI()


def _upsert_job_match(
    db: Session,
    applicant_id: Any,
    job_id: Any,
    result: dict,
) -> None:
    stmt = insert(JobMatch).values(
        **{
        "ApplicantId" : applicant_id,
        "JobId" : job_id,
        "MatchScore" : result["match_score"],
        "Reason" : result["reason"],
        }
    )

    stmt = stmt.on_conflict_do_update(
        index_elements=[
            "ApplicantId",
            "JobId"
        ],
        set_={
            "MatchScore": stmt.excluded.MatchScore,
            "Reason": stmt.excluded.Reason,
        },
    )
    db.execute(stmt)

def _calculate_match_for_pair(
    db: Session,
    applicant_id: int,
    job_id: int,
) -> dict:
    applicant = db.query(Applicant).filter(
        Applicant.applicant_id == applicant_id
    ).first()

    if not applicant:
        raise HTTPException(status_code=404, detail="Applicant not found")

    job = db.query(Job).filter(Job.job_id == job_id).first()

    if not job:
        raise HTTPException(status_code=404, detail="Job not found")

    applicant_skills = db.query(ApplicantSkill).filter(
        ApplicantSkill.applicant_id == applicant_id
    ).all()

    job_skills = db.query(JobSkill).filter(
        JobSkill.job_id == job_id
    ).all()

    result = calculate_match(
        applicant,
        applicant_skills,
        job,
        job_skills,
    )

    _upsert_job_match(
        db=db,
        applicant_id=applicant_id,
        job_id=job_id,
        result=result,
    )

    db.commit()

    return result


@app.post("/match/applicant/{applicant_id}")
def match_applicant_to_all_jobs(
    applicant_id: int,
    db: Session = Depends(get_db),
):
    applicant = db.query(Applicant).filter(
        Applicant.applicant_id == applicant_id
    ).first()

    if not applicant:
        raise HTTPException(status_code=404, detail="Applicant not found")

    applicant_skills = db.query(ApplicantSkill).filter(
        ApplicantSkill.applicant_id == applicant_id
    ).all()

    jobs = db.query(Job).all()
    results = []

    for job in jobs:
        job_skills = db.query(JobSkill).filter(
            JobSkill.job_id == job.job_id
        ).all()

        result = calculate_match(
            applicant,
            applicant_skills,
            job,
            job_skills,
        )

        _upsert_job_match(
            db=db,
            applicant_id=applicant_id,
            job_id=job.job_id,
            result=result,
        )

        results.append(
            {
                "job_id": job.job_id,
                "match_score": result["match_score"],
                "reason": result["reason"],
                "breakdown": result["breakdown"],
                "recommendations": result["recommendations"],
            }
        )

    db.commit()

    return {
        "applicant_id": applicant_id,
        "matches": results,
    }


@app.post("/match/job/{job_id}")
def match_job_to_all_applicants(
    job_id: int,
    db: Session = Depends(get_db),
):
    job = db.query(Job).filter(Job.job_id == job_id).first()

    if not job:
        raise HTTPException(status_code=404, detail="Job not found")

    job_skills = db.query(JobSkill).filter(JobSkill.job_id == job_id).all()
    applicants = db.query(Applicant).all()
    results = []

    for applicant in applicants:
        applicant_skills = db.query(ApplicantSkill).filter(
            ApplicantSkill.applicant_id == applicant.applicant_id
        ).all()

        result = calculate_match(
            applicant,
            applicant_skills,
            job,
            job_skills,
        )

        _upsert_job_match(
            db=db,
            applicant_id=applicant.applicant_id,
            job_id=job_id,
            result=result,
        )

        results.append(
            {
                "applicant_id": applicant.applicant_id,
                "match_score": result["match_score"],
                "reason": result["reason"],
                "breakdown": result["breakdown"],
                "recommendations": result["recommendations"],
            }
        )

    db.commit()

    return {
        "job_id": job_id,
        "matches": results,
    }


@app.get("/match/{applicant_id}/{job_id}")
def get_match(
    applicant_id: int,
    job_id: int,
    db: Session = Depends(get_db),
):
    match = db.query(JobMatch).filter_by(
        applicant_id=applicant_id,
        job_id=job_id,
    ).first()

    if not match:
        result = _calculate_match_for_pair(db, applicant_id, job_id)
        return {
            "applicant_id": applicant_id,
            "job_id": job_id,
            "match_score": result["match_score"],
            "reason": result["reason"],
        }

    return {
        "applicant_id": match.applicant_id,
        "job_id": match.job_id,
        "match_score": match.match_score,
        "reason": match.reason,
    }



@app.post("/parse-resume")
async def parse_resume(file: UploadFile = File(...), db: Session = Depends(get_db)):
    if file.content_type != "application/pdf":
        raise HTTPException(
            status_code=400,
            detail="only Pdf files are supported",
        )
    contents = await file.read()

    try:
        with pdfplumber.open(io.BytesIO(contents)) as pdf:
            text = "\n".join(
                page.extract_text() or ""
                for page in pdf.pages
            )
    except Exception as e:
        raise HTTPException(
            status_code=400,
            detail=f"Could not read PDF: {str(e)}"
        )
    normalized_text = text.lower()
    skills = db.query(Skill).all()
    matched_skills =[]
    for skill in skills:
        if not skill.skill_name:
            continue
        skill_name = skill.skill_name.lower()

        if skill_name in normalized_text:
            matched_skills.append(
                {
                    "skillId": skill.skill_id,
                    "skillName": skill.skill_name,
                }
            )
    experience_years = None

    experience_patterns = [
    r"(\d+)\+?\s*years?\s*(?:of\s*)?(?:professional\s*)?experience",
    r"experience\s*[:\-]?\s*(\d+)\+?\s*years?",
    r"(\d+)\+?\s*years?\s*experience",
    ]
    for pattern in experience_patterns:
        match = re.search(
            pattern,
            normalized_text,
        )

        if match:
            experience_years = int(match.group(1))
            break

    doc = nlp(text)
    location = None
    for ent in doc.ents:
        if ent.label_ in ("GPE", "LOC"):
            location = ent.text
            break
    return{
        "filename": file.filename,
        "skills": matched_skills,
        "experienceYears": experience_years,
        "text": text,
        "location": location
    }