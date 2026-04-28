using SkillCypher.Core.DTOs.Applicant;

namespace SkillCypher.Core.Interfaces
{
    public interface IApplicantService
    {
        Task<ApplicantProfileDto?> GetProfileAsync(int userId);
        Task<ApplicantProfileDto?> UpdateProfileAsync(
            int userId,
            UpdateApplicantProfileDto updateDto);

        Task AddSkillAsync(int userId, int skillId);
        Task RemoveSkillAsync(int userId, int skillId);
    }
}