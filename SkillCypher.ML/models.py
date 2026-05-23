#models.py
from sqlalchemy import (
    Column,
    Integer,
    String,
    Float,
    Boolean,
    Text,
    ForeignKey,
    PrimaryKeyConstraint
)

from sqlalchemy.orm import(
    declarative_base,
    relationship
)

Base = declarative_base()

class User(Base):
    __tablename__ = "Users"

    user_id = Column(
        "UserId",
        Integer,
        primary_key=True,
        index=True
    )

    name = Column("Name",String)

    email = Column("Email",String)

    applicant = relationship(
        "Applicant",
        back_populates="user"
    )
class Applicant(Base):
    __tablename__ = "Applicants"

    applicant_id = Column(
        "ApplicantId",
        Integer,
        primary_key=True,
        index=True
    )

    user_id = Column(
        "UserId",
        Integer,
        ForeignKey("Users.UserId")
    )

    experience = Column(
        "Experience",
        Integer
    )

    preferred_location = Column(
        "PreferredLocation",
        String
    )

    user = relationship(
        "User",
        back_populates="applicant"
    )

    skills = relationship(
        "ApplicantSkill",
        back_populates="applicant"
    )
class Skill(Base):
    __tablename__ = "Skills"

    skill_id = Column(
        "SkillId",
        Integer,
        primary_key=True,
        index=True
    )

    skill_name = Column(
        "SkillName",
        String
    )

    applicant_skills = relationship(
        "ApplicantSkill",
        back_populates="skill"
    )
    job_skills = relationship(
        "JobSkill",
        back_populates="skill"
    )

class ApplicantSkill(Base):
    __tablename__ = "ApplicantSkills"

    applicant_id = Column(
        "ApplicantId",
        Integer,
        ForeignKey("Applicants.ApplicantId")
    )

    skill_id = Column(
        "SkillId",
        ForeignKey("Skills.SkillId")
    )

    __table_args__ = (
        PrimaryKeyConstraint(
            "ApplicantId",
            "SkillId"
        ),
    )

    applicant = relationship(
        "Applicant",
        back_populates="skills"
    )

    skill = relationship(
        "Skill",
        back_populates="applicant_skills"
    )

class Job(Base):
    __tablename__ = "Jobs"

    job_id = Column(
        "JobId",
        Integer,
        primary_key=True,
        index=True
    )

    location = Column(
        "Location",
        String
    )

    min_salary = Column(
        "MinSalary",
        Float
    )
    max_salary = Column(
        "MaxSalary",
        Float
    )

    required_experience_years = Column(
        "RequiredExperienceYears",
        Integer
    )

    skills = relationship(
        "JobSkill",
        back_populates="job"
    )
class JobSkill(Base):
    __tablename__ = "JobSkills"

    job_id = Column(
        "JobId",
        Integer,
        ForeignKey("Jobs.JobId")
    )

    skill_id = Column(
        "SkillId",
        Integer,
        ForeignKey("Skills.SkillId")
    )

    is_required = Column(
        "IsRequired",
        Boolean
    )

    __table_args__ = (
        PrimaryKeyConstraint(
            "JobId",
            "SkillId"
        ),
    )

    job = relationship(
        "Job",
        back_populates="skills"
    )

    skill = relationship(
        "Skill",
        back_populates="job_skills"
    )

class JobMatch(Base):
    __tablename__ = "JobMatches"

    applicant_id = Column(
        "ApplicantId",
        Integer,
        ForeignKey("Applicants.ApplicantId"),
        nullable=False
    )
    job_id = Column(
        "JobId",
        Integer,
        ForeignKey("Jobs.JobId"),
        nullable=False
    )
    match_score = Column(
        "MatchScore",
        Float,
        nullable=False
    )

    reason = Column(
        "Reason",
        Text,
        nullable=False
    )

    __table_args__ = (
        PrimaryKeyConstraint(
            "ApplicantId",
            "JobId"
        ),
    )