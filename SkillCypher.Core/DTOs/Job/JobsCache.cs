namespace SkillCypher.Core.DTOs.Job
{
    public class JobsCache
    {
        public List<JobListItemDto> Jobs { get; set; } = new();
        public int TotalCount { get; set; }
    }
}