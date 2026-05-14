using SkillCypher.Core.Models;

namespace SkillCypher.Core.DTOs.Application
{
    public class ApplicationResponseDto
    {
        public int ApplicationId {get;set;}
        public int ApplicantId {get;set;}
        public string ApplicantName { get; set; } = string.Empty;
        public string ApplicantEmail { get; set; } = string.Empty;
        public int JobId {get;set;}
        public string ResumeUrl {get;set;} = string.Empty;
        public DateTime AppliedAt {get;set;}
        public ApplicationStatus Status {get;set;}
    }
}