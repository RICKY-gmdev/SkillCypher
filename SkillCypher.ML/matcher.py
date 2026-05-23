#matcher.py
from typing import Dict
SKILL_WEIGHT = 0.50
EXPERIENCE_WEIGHT = 0.20
LOCATION_WEIGHT = 0.15
CERTIFICATE_WEIGHT = 0.15
def calculate_experience_score(
        applicant_experience: int,
        required_experience: int
) -> float:
    if required_experience <=0:
        return 1.0
    if applicant_experience >= required_experience:
        return 1.0
    difference = abs(
        required_experience - applicant_experience
    )
    score = 1 - (
        difference /max(required_experience,1)
    )
    return max(0.0,score)
def calculate_location_score(
    applicant_location: str,
    job_location: str
) -> float:
    if not applicant_location or not job_location:
        return 0.5    
    applicant_location = applicant_location.strip().lower()
    job_location = job_location.strip().lower()
    return 1.0 if applicant_location == job_location else 0.0
def calculate_match(
        applicant,
        applicant_skills,
        job,
        job_skills
) -> Dict:
    applicant_skills_ids = set(
        s.skill_id for s in applicant_skills
    )

    required_skills = [
        s for s in job_skills if s.is_required
    ]

    required_skills_ids = set(
        s.skill_id for s in required_skills
    )

    all_job_skill_ids = set(
        s.skill_id for s in job_skills
    )

    matched_skill_ids = (
        required_skills_ids & applicant_skills_ids
    )

    missing_skill_ids = (
        required_skills_ids -
        applicant_skills_ids
    )

    missing_skill_names = [
    skill.skill.skill_name
    for skill in required_skills
    if skill.skill_id in missing_skill_ids
]

    skill_score = (
        len(matched_skill_ids) /
        len(required_skills_ids)
    ) if required_skills_ids else 1.0

    applicant_experience = getattr(
        applicant,
        "experience",
        0
    ) or 0

    required_experience = getattr(
        job,
        "required_experience_years",
        0
    ) or 0

    experience_score = calculate_experience_score(
        applicant_experience,
        required_experience
    )

    applicant_location = getattr(
        applicant,
        "preferred_location",
        ""
    )

    job_location = getattr(
        job,
        "location",
        ""
    )

    location_score = calculate_location_score(
        applicant_location,
        job_location
    )

    # TODO: replace this placeholder with real certificate comparison logic.
    certificate_score = 1.0


    final_score = (
        (skill_score * SKILL_WEIGHT) +
        (experience_score * EXPERIENCE_WEIGHT) +
        (location_score * LOCATION_WEIGHT) +
        (certificate_score * CERTIFICATE_WEIGHT)
    )

    final_score = round(final_score * 100, 2)

    matched_skills_count = len(matched_skill_ids)
    total_required_skills = len(required_skills_ids)

    if skill_score >= 0.8:
        skill_feedback = "strong skill alignment."
    elif skill_score >= 0.5:
        skill_feedback = "partial skill alignment."
    else:
        skill_feedback = "Significant skill gap detected."

    if experience_score >= 0.8:
        experience_feedback = "experience level is a strong match."
    elif experience_score >= 0.5:
        experience_feedback = "experience level is acceptable."
    else:
        experience_feedback = "experience level is below requirement."

    if location_score == 1.0:
        location_feedback = "Preferred location matches the job location."
    else:
        location_feedback = "Preferred location differs from the job location."

    recommendations = []


    if missing_skill_names:
        recommendations.append(
            f"Learning {', '.join(missing_skill_names)} "
            f"could significantly improve your eligibility in this role."
        )
    if experience_score < 0.5:
        recommendations.append(
            "Gain more practical experience in this domain."
        )

    if location_score == 0.0:
        recommendations.append(
            "Consider remote or relocation opportunities."
        )

    reason = (
        f"{skill_feedback} "
        f"{experience_feedback} "
        f"{location_feedback}"
    )

    return {
        "match_score": final_score,

        "breakdown": {
            "skill_score": round(skill_score * 100, 2),
            "experience_score": round(experience_score * 100, 2),
            "location_score": round(location_score * 100, 2),
            "certificate_score": round(certificate_score * 100, 2)
        },

        "matched_skill_count": matched_skills_count,

        "required_skill_count": total_required_skills,

        "matched_skill_ids": list(matched_skill_ids),

        "missing_skill_ids": list(missing_skill_ids),

        "reason": reason,

        "recommendations": recommendations
    }