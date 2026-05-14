namespace SkillCypher.Core.DTOs.Job
{
    public class CreateJobDto
    {
        public string Title{get;set;} = string.Empty;
        public string Description{get;set;} = string.Empty;
        public decimal? MinSalary {get;set;}
        public decimal? MaxSalary {get;set;} 
        public int CompanyId{get;set;}
        public string Location {get;set;} = string.Empty;
        public string Requirements {get;set;} = string.Empty;
        public string JobType {get;set;} = string.Empty;
        public List<int> SkillIds { get; set; } = new();
        public List<int> CertificateIds { get; set; } = new();
    }
}