using SkillCypher.Core.DTOs.Recruiter;

namespace SkillCypher.Core.Interfaces
{
    public interface IRecruiterRepository
    {
        Task<RecruiterDashboardDto?> GetDashboardAsync(int userId);
    }
}