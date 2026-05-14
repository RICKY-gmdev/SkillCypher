namespace SkillCypher.Core.DTOs.Application
{
    public class CreateApplicationDto
    {
        public int JobId {get; set;}
        public string ResumeUrl {get; set;} =string.Empty;
    }
}