using SkillCypher.Core.DTOs.Job;

namespace SkillCypher.Core.Interfaces
{
    public interface IJobService
    {
        Task<(IEnumerable<JobListItemDto> Jobs,int TotalCount)> GetJobsAsync(JobQueryParams queryParams);
        Task<JobResponseDto> CreateJobAsync(CreateJobDto createJobDto, int recruiterId);
        Task<JobResponseDto?> GetJobByIdAsync(int jobId);
        Task<JobResponseDto?> UpdateJobAsync(int jobId, CreateJobDto updateJobDto,int recruiterId);
        Task<bool> DeleteJobAsync(int jobId, int recruiterId);
        Task<int?> GetRecruiterIdByuserIdAsync(int userId);
        Task<IEnumerable<JobListItemDto>> GetJobsByRecruiterIdAsync(int recruiterId);
    }
}