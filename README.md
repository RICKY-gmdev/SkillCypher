# SkillCypher

**Skill-based recruitment. Better matches. Clearer decisions.**

SkillCypher is an intelligent recruitment platform designed to connect applicants with job opportunities based on skills, experience, location, and job requirements. It combines a recruitment management API, an Angular web client, and a Python-based matching engine to help applicants discover relevant opportunities and recruiters evaluate candidates.

Rather than treating recruitment as a keyword-matching exercise, SkillCypher aims to make candidate-job compatibility easier to understand through match scores, skill comparisons, and recommendations.

> **Project status:** In active development. Core API features and matching functionality are implemented, while the frontend experience and some advanced workflows are still being developed.

## Features

### For Applicants

- **Job discovery:** Browse available job listings, search by keywords, filter by location, and navigate paginated results.
- **Skill-based matching:** Receive compatibility scores based on applicant skills and job requirements, alongside experience and location factors.
- **Applicant dashboard:** View profile-related statistics, application counts, and recommended job matches.
- **Profile and skill management:** Select skills, maintain profile information, and synchronize skill changes with the matching service.
- **Application management:** Apply for jobs, review application history, and withdraw applications.
- **Resume analysis foundation:** A Python API endpoint can extract text from PDF resumes and identify potential skills, experience, and location information.

### For Recruiters

- **Company management:** Create company profiles and associate recruiters with companies.
- **Job management:** Create, update, and delete job listings, with recruiter-specific job retrieval.
- **Candidate matching:** View candidate match information and compatibility scores through the matching infrastructure.
- **Application management:** Retrieve applications for recruiter-owned jobs and update application statuses.

### Matching Engine

The Python matching service calculates an overall compatibility score using a weighted, multi-factor approach. It also produces explanatory feedback and recommendations.

The current weights are:

| Factor | Weight |
|---|---:|
| Required-skill alignment | 50% |
| Experience alignment | 20% |
| Location compatibility | 15% |
| Certification score | 15% |

**Implementation note:** Certification scoring is currently a placeholder rather than actual certificate comparison or verification. The scoring model and weights may evolve as the matching engine develops.

## Architecture

SkillCypher is organized as a layered .NET solution with a separate Angular client and Python matching service.

```text
SkillCypher/
├── SkillCypher.API/             # ASP.NET Core REST API
│   └── Controllers/
├── SkillCypher.Client/          # Angular web frontend
│   └── src/app/
│       ├── components/
│       ├── services/
│       └── interceptors/
├── SkillCypher.Core/            # Core models, DTOs, interfaces and services
├── SkillCypher.Infrastructure/  # Data access, repositories and integrations
│   ├── Data/
│   ├── Migrations/
│   ├── Repositories/
│   └── Services/
├── SkillCypher.ML/              # FastAPI matching and resume analysis
│   ├── main.py
│   ├── matcher.py
│   ├── models.py
│   └── database.py
├── SkillCypher.Worker/          # .NET background worker scaffold
├── SkillCypher.sln
├── LICENSE
└── README.md
```

### Technology stack

| Layer | Technologies |
|---|---|
| Frontend | Angular, TypeScript, HTML, CSS |
| Main API | ASP.NET Core Web API, C# |
| Application architecture | Layered design, dependency injection, repositories and services |
| ORM and database access | Entity Framework Core, Npgsql, SQLAlchemy |
| Database | PostgreSQL |
| Matching and document processing | Python, FastAPI, spaCy, pdfplumber |
| Authentication | JWT bearer authentication, role-based authorization |
| Caching infrastructure | Redis, StackExchange.Redis |
| Background processing foundation | .NET Worker Service |

## How the matching process works

The current matching algorithm evaluates several factors:

1. **Skills:** Compares an applicant's skills with the skills marked as required by a job.
2. **Experience:** Calculates an experience-alignment score using the applicant's and job's experience values.
3. **Location:** Compares the applicant's preferred location with the job location.
4. **Certifications:** Currently uses a placeholder score until real certificate comparison is implemented.

The weighted scores are combined into an overall percentage. The matching service can also return matched and missing skill identifiers, an explanation of the result, and recommendations for addressing skill gaps.

The API supports calculating matches for an applicant across jobs, for a job across applicants, and retrieving an individual applicant-job match.


## API overview

The main ASP.NET Core API exposes endpoints for authentication, applicants, skills, jobs, companies, recruiters, applications, and match information.

The separate FastAPI service currently exposes these matching and resume-analysis endpoints:

| Method | Endpoint | Purpose |
|---|---|---|
| `POST` | `/match/applicant/{applicant_id}` | Calculate and persist an applicant's matches across jobs |
| `POST` | `/match/job/{job_id}` | Calculate and persist matches for a job across applicants |
| `GET` | `/match/{applicant_id}/{job_id}` | Retrieve an individual match, calculating it if needed |
| `POST` | `/parse-resume` | Extract information and potential skills from a PDF resume |

The main API uses JWT authentication and role-based authorization for protected operations.

For endpoint details and request formats, inspect the controllers in `SkillCypher.API/Controllers` and the route handlers in `SkillCypher.ML/main.py`.

## Development status

The repository contains the main building blocks of the application, but not every planned feature is complete.

Current areas of development include:

- Completing and unifying the applicant and recruiter frontend experiences.
- Improving dashboard presentation and navigation between workflows.
- Integrating resume analysis into the applicant onboarding process.
- Completing candidate review and job-management interfaces.
- Replacing placeholder certification scoring with meaningful comparisons.
- Expanding background-processing behavior beyond the current worker scaffold.

The `.NET Worker` project currently serves as a basic hosted background service. It should not be considered a completed job-processing pipeline.

## Design principles

SkillCypher is being developed around a few guiding ideas:

- **Skills over keywords alone:** Evaluate relevant capabilities rather than relying exclusively on matching job titles.
- **Explainability:** Make match scores easier to interpret with skill-alignment feedback.
- **Clear workflows:** Give applicants and recruiters interfaces suited to their respective tasks.
- **Incremental development:** Keep the application maintainable by improving existing functionality before adding unnecessary complexity.


## License

See the [LICENSE](LICENSE) file for the project's license.

---

**SkillCypher**  
*Better matches begin with understanding skills.*
