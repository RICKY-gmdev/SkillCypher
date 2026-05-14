namespace SkillCypher.Core.Models
{
    public enum ApplicationStatus
    {
        Applied,
        UnderReview,
        ShortListed,
        Rejected,
        Accepted,
        Withdrawn
    }
public class Application
    {
        public int ApplicationId {get;set;}

        public int ApplicantId{get;set;}
        public Applicant Applicant {get;set;} = null!;

        public int JobId{get;set;}
        public Job Job {get;set;} = null!;

        public string? ResumeUrl {get;set;}

        public DateTime AppliedAt {get;set;}

        public ApplicationStatus Status {get;set;}
        
    }

}