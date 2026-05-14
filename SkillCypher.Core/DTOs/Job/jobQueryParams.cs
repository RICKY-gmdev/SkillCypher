namespace SkillCypher.Core.DTOs.Job
{
    public class JobQueryParams
    {
        
        public string? Search { get; set; }
        public string? Location { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
    }
}