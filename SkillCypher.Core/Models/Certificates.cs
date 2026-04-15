namespace SkillCypher.Core.Models
{
    public class Certificate
    {
        public int CertificateId {get;set;}
        public string CertificateName {get;set;} = string.Empty;
        public string IssuingBody {get;set;} = string.Empty;
        public DateTime? ExpiryPeriod{get;set;}
    }
}