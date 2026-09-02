using SkillCypher.Core.DTOs.Recruiter;

namespace SkillCypher.Core.Interfaces
{
    public interface IRecruiterService
    {
        Task<RecruiterDashboardDto?> GetDashboardAsync(int userId);
    }
}