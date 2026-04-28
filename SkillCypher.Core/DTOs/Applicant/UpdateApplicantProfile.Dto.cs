namespace SkillCypher.Core.DTOs.Applicant
{
    public class UpdateApplicantProfileDto
    {
        public string ResumeUrl {get; set;} = string.Empty;
        public int Experience {get;set;}
        public string? PreferredLocation { get;set;}
    }
}