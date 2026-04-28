namespace SkillCypher.Core.Models
{

public class Job
{
    public int JobId {get;set;}
    public int RecruiterId {get; set;}
    public Recruiter Recruiter {get;set;} = null!;
    public int CompanyId{get;set;}
    public Company Company {get;set;} = null!;
    public string? Title{get;set;}
    public string? Location{get;set;}
    public decimal? MinSalary{get;set;}
    public decimal? MaxSalary{get;set;}
    public string? Requirements{get;set;}
    public string? JobType {get;set;}
    public DateTime? PostedAt {get;set;}
      public ICollection<JobSkill> JobSkills { get; set; }
            = new List<JobSkill>();

        public ICollection<JobCertificate> JobCertificates { get; set; }
            = new List<JobCertificate>();
}
}