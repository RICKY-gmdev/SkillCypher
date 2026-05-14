using SkillCypher.Core.DTOs.Job;
using SkillCypher.Core.Models;

namespace SkillCypher.Core.Interfaces
{
    public interface IJobRepository
    {
        Task<(IEnumerable<Job> Jobs, int TotalCount)> GetJobsAsync(JobQueryParams queryParams);

        Task<Job> CreateJobAsync(Job job);

        Task<Job?> GetJobByIdAsync(int jobId);

        Task<Job?> UpdateJobAsync(Job job);

        Task<bool> DeleteJobAsync(int jobId);

        Task<Recruiter?> GetRecruiterIdByUserIdAsync(int userId);

        Task<IEnumerable<Job>> GetJobsByRecruiterIdAsync(int recruiterId);
    }
}