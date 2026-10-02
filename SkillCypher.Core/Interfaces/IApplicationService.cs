using SkillCypher.Core.DTOs.Application;
using SkillCypher.Core.Models;

namespace SkillCypher.Core.Interfaces
{
    public interface IApplicationService
    {
        Task<ApplicationResponseDto> ApplyToJobAsync(int applicantId,CreateApplicationDto dto);
        Task<ApplicationResponseDto?> GetApplicationByIdAsync(int applicationId);
        Task<IEnumerable<ApplicationResponseDto>> GetApplicationByApplicantIdAsync(int applicantId);
        Task<IEnumerable<ApplicationResponseDto>> GetApplicationByJobIdAsync(int jobId);
        Task WithdrawApplicationAsync(int applicationId, int applicantId);
        Task<bool> HasApplicantAppliedAsync(int applicantId, int jobId);
        Task<ApplicationResponseDto?> UpdateApplicationStatusAsync(int applicationId, int recruiterId, ApplicationStatus status);
    }
}