namespace SkillCypher.Core.Models{

public class Applicant
{
    public int ApplicantId {get; set;}
    public int UserId {get; set;}
    public User User {get;set;} = null!;
    public string ResumeUrl {get; set;} = string.Empty;
    public double Experience {get; set;}//years
    public string? PreferredLocation {get;set;}
    public ICollection<ApplicantSkill> ApplicantSkills { get; set; } = new List<ApplicantSkill>();
    public ICollection<ApplicantCertificate> ApplicantCertificates { get; set; } = new List<ApplicantCertificate>();
}
}