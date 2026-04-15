namespace SkillCypher.Core.Models
{
    public class JobMatch
    {
        public int ApplicantId {get;set;}

        public Applicant Applicant {get;set;} = null!;


        public int JobId {get;set;}
        public Job Job {get;set;} = null!;

        public decimal MatchScore {get;set;}

        public string Reason {get;set;} = string.Empty;
    }
}