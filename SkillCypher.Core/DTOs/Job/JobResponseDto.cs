using SkillCypher.Core.DTOs.Applicant;

namespace SkillCypher.Core.DTOs.Job
{
    public class JobResponseDto
    {
        public int JobId { get; set; }
        public int RecruiterId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int RequiredExperienceYears { get; set; }
        public string Requirements { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public DateTime PostedAt { get; set; }
        public List<SkillDto> Skills { get; set; } = new();
        public List<CertificateDto> Certificates { get; set; } = new();
    }
}