from typing import Any

from fastapi import Depends, FastAPI, HTTPException
from sqlalchemy.orm import Session

from database import get_db
from matcher import calculate_match
from models import Applicant, ApplicantSkill, Job, JobMatch, JobSkill

app = FastAPI()


def _upsert_job_match(
    db: Session,
    applicant_id: Any,
    job_id: Any,
    result: dict,
) -> None:
    existing = db.query(JobMatch).filter_by(
        applicant_id=applicant_id,
        job_id=job_id,
    ).first()

    if existing:
        existing.match_score = result["match_score"]
        existing.reason = result["reason"]
        return

    db.add(
        JobMatch(
            applicant_id=applicant_id,
            job_id=job_id,
            match_score=result["match_score"],
            reason=result["reason"],
        )
    )


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