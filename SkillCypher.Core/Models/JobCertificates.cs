namespace SkillCypher.Core.Models
{
    public class JobCertificate
    {
        public int JobId {get;set;}
        public Job Job {get;set;} = null!;
        public int CertificateId {get;set;}
        public Certificate Certificates {get;set;} = null!;
        public bool IsRequired{get;set;}
        }
}