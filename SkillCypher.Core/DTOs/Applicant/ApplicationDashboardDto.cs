namespace SkillCypher.Core.DTOs.Applicant
{
    public class ApplicantDashboardDto
    {
        public string ApplicantName { get; set; } = string.Empty;
        public int TotalSkills { get; set; }
        public int TotalApplications { get; set; }
        public string? PreferredLocation { get; set; }
        public List<JobMatchSummaryDto> JobMatches { get; set; } = new();
    }

    public class JobMatchSummaryDto
    {
        public int JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public double MatchScore { get; set; }
        public int MatchedSkillCount { get; set; }
        public int RequiredSkillCount { get; set; }
    }
}