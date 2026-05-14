namespace SkillCypher.Core.DTOs.Job
{
    public class JobListItemDto
    {
        public int JobId {get;set;}
        public string Title{get;set;} = string.Empty;
        public decimal? MinSalary {get;set;}
        public decimal? MaxSalary {get;set;}
        public int CompanyId{get;set;}
        public string CompanyName {get;set;} = string.Empty;
    }
}