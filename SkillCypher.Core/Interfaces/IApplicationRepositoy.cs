using SkillCypher.Core.Models;

namespace SkillCypher.Core.Interfaces
{
    public interface IApplicationRepository
    {
        Task <Application> ApplyToJobAsync(Application application);
        Task<Application?> GetApplicationByIdAsync(int applicationId);
        Task<IEnumerable<Application>> GetApplicationByApplicantIdAsync(int applicantId);
        Task<IEnumerable<Application>> GetApplicationByJobIdAsync(int jobId);
        Task WithdrawApplicationAsync(int applicationId,int applicantId);
        Task<bool> HasApplicantAppliedAsync(int applicantId, int jobId);
    }
}