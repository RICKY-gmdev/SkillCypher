namespace SkillCypher.Core.DTOs.Applicant
{
    public class CertificateDto
    {
        public int CertificateId{ get; set; }
        public string CertificateName {get; set;} = string.Empty;
        public string IssuedBy { get; set;} = string.Empty;
        public DateTime? IssuedAt {get;set;}
        public string? CertificateUrl { get; set;}
    }
}