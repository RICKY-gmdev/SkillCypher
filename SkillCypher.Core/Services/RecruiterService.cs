using SkillCypher.Core.DTOs.Recruiter;
using SkillCypher.Core.Interfaces;

namespace SkillCypher.Core.Services
{
    public class RecruiterService : IRecruiterService
    {
        private readonly IRecruiterRepository _recruiterRepository;
        public RecruiterService(IRecruiterRepository recruiterRepository)
        {
            _recruiterRepository = recruiterRepository;
        }

        public async Task<RecruiterDashboardDto?> GetDashboardAsync(int userId)
        {
            return await _recruiterRepository.GetDashboardAsync(userId);
        }
    }
}