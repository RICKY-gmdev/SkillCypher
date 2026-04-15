namespace SkillCypher.Core.Models
{
    public class ApplicantCertificate
    {
        public int ApplicantId {get;set;}
        public Applicant Applicant {get;set;} = null!;

        public int CertificateId {get;set;}
        public Certificate Certificates {get;set;} = null!;
        public DateTime? IssuedDate {get;set;}
        public DateTime? ExpiryDate {get;set;}
        public string? CertificateUrl {get;set;}
    }
}