using SkillCypher.Core.Models;

namespace SkillCypher.Core.Interfaces
{
    public interface IApplicantRepository
    {
        Task<Applicant?> GetProfileAsync(int UserId);
        Task<Applicant> UpdateApplicantAsync(Applicant applicant);
        Task AddSkillAsync(int applicantId, int skillId);
        Task RemoveSkillAsync(int applicantId, int skillId);

    }
}