namespace SkillCypher.Core.DTOs.Recruiter
{
    public class RecruiterDashboardDto
    {
        public string RecruiterName { get; set; } = string.Empty;
        public int TotalJobsPosted { get; set; }
        public int TotalApplicationsReceived { get; set; }

        public List<RecruiterJobDashboardDto> Jobs { get; set; } = new();
    }

    public class RecruiterJobDashboardDto
    {
        public int JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TotalApplicants { get; set; }
        public double TopMatchScore { get; set; }

        public List<RecruiterApplicationMatchDto> Applicants { get; set; } = new();
    }

    public class RecruiterApplicationMatchDto
    {
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public double MatchScore { get; set; }
    }
}