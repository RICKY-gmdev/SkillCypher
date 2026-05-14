using SkillCypher.Core.DTOs.Application;
using SkillCypher.Core.Interfaces;
using SkillCypher.Core.Models;

namespace SkillCypher.Core.Services
{
    public class ApplicationService : IApplicationService
    {

        private readonly IApplicationRepository _applicationRepository;
        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }
        public async Task<ApplicationResponseDto> ApplyToJobAsync(int applicantId, CreateApplicationDto dto)
        {
            bool alreadyCreated = await _applicationRepository.HasApplicantAppliedAsync(applicantId, dto.JobId);
            if (alreadyCreated)
            {
                throw new Exception("Applicant has already applied for this job.");
            }

            var application = new Application
            {
                ApplicantId = applicantId,
                JobId = dto.JobId,
                ResumeUrl = dto.ResumeUrl,
                AppliedAt = DateTime.UtcNow,
                Status = ApplicationStatus.Applied
            };

            var createApplication = await _applicationRepository.ApplyToJobAsync(application);

            return new ApplicationResponseDto
            {
                ApplicationId = createApplication.ApplicationId,
                ApplicantId = createApplication.ApplicantId,
                ApplicantName = string.Empty,
                ApplicantEmail = string.Empty,
                JobId = createApplication.JobId,
                ResumeUrl = createApplication.ResumeUrl ?? string.Empty,
                AppliedAt = createApplication.AppliedAt,
                Status = createApplication.Status
            };
        }

        public async Task<IEnumerable<ApplicationResponseDto>> GetApplicationByApplicantIdAsync(int applicantId)
        {
            var applications = await _applicationRepository.GetApplicationByApplicantIdAsync(applicantId);
            return applications.Select(MapToResponseDto);
        }

        public async Task<ApplicationResponseDto?> GetApplicationByIdAsync(int applicationId)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(applicationId);
            if (application == null)
            {
                return null;
            }
            return MapToResponseDto(application);
        }

        public async Task<IEnumerable<ApplicationResponseDto>> GetApplicationByJobIdAsync(int jobId)
        {
            var applications = await _applicationRepository.GetApplicationByJobIdAsync(jobId);

            return applications.Select(MapToResponseDto);
        }

        public async Task<bool> HasApplicantAppliedAsync(int applicantId, int jobId)
        {
            return await _applicationRepository.HasApplicantAppliedAsync(applicantId, jobId);
        }

        public async Task WithdrawApplicationAsync(int applicationId, int applicantId)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(applicationId);
            if (application == null)
            {
                throw new Exception("Application not found.");
            }
            if (application.ApplicantId != applicantId)
            {
                throw new Exception("Unauthorized withdrawal attempt.");
            }

            await _applicationRepository.WithdrawApplicationAsync(applicationId, applicantId);
        }

        private static ApplicationResponseDto MapToResponseDto(Application application)
        {
            return new ApplicationResponseDto
            {
                ApplicationId = application.ApplicationId,
                ApplicantId = application.ApplicantId,
                ApplicantName = application.Applicant?.User?.Name ?? string.Empty,
                ApplicantEmail = application.Applicant?.User?.Email ?? string.Empty,
                JobId = application.JobId,
                ResumeUrl = application.ResumeUrl ?? string.Empty,
                AppliedAt = application.AppliedAt,
                Status = application.Status
            };
        }
    }
}