namespace SkillCypher.Core.DTOs.Applicant
{
    public class ApplicantProfileDto
    {
        public string Name{get;set;} = string.Empty;
        public string Email {get;set;} =string.Empty;
        public string? ResumeUrl {get;set;}
        public int Experience {get;set;}
        public string? PreferredLocation{get;set;}

        public List<SkillDto> Skills {get;set;} = new();
        public List<CertificateDto> Certificates {get;set;} = new();
    }
}