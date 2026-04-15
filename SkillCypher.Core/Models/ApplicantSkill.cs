namespace SkillCypher.Core.Models
{
    public class ApplicantSkill
    {
        public int ApplicantId{get;set;}
        public Applicant Applicant {get;set;}  =null!;

        public int SkillId {get;set;}
        public Skill Skill {get;set;} = null!;
    }
}