using SkillCypher.Core.Models;

namespace SkillCypher.Core.Interfaces
{
    public interface IJobMatchRepository
    {
        Task<JobMatch?> GetJobMatchAsync(int applicantId,int jobId);
    }
}