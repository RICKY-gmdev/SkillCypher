using SkillCypher.Core.DTOs.Applicant;
using SkillCypher.Core.Interfaces;

namespace SkillCypher.Core.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IMatchingService _matchingService;
        public ApplicantService(IApplicantRepository applicantRepository, IMatchingService matchingService)
        {
            _applicantRepository = applicantRepository;
            _matchingService = matchingService;
        }
        public async Task<ApplicantProfileDto?> GetProfileAsync(int userId)
        {
            var applicant = await _applicantRepository.GetProfileAsync(userId);
            if (applicant == null)
            {
                return null;
            }
            return new ApplicantProfileDto
            {
                ApplicantId = applicant.ApplicantId,
                Name = applicant.User.Name,
                Email = applicant.User.Email,
                ResumeUrl = applicant.ResumeUrl,
                Experience = (int)applicant.Experience,
                PreferredLocation = applicant.PreferredLocation,

                Skills = applicant.ApplicantSkills
                .Select(s => new SkillDto
                {
                    SkillId = s.SkillId,
                    SkillName = s.Skill.SkillName
                })
                .ToList(),

                Certificates = applicant.ApplicantCertificates?
                .Select(c => new CertificateDto
                {
                    CertificateId = c.CertificateId,
                    CertificateName = c.Certificate.CertificateName,
                    IssuedBy = c.Certificate.IssuingBody,
                    CertificateUrl = c.CertificateUrl
                })
                    .ToList() ?? new List<CertificateDto>()
            };
        }
        public async Task<ApplicantProfileDto?> UpdateProfileAsync(int userId, UpdateApplicantProfileDto updateDto)
        {
            var applicant = await _applicantRepository.GetProfileAsync(userId);
            if (applicant == null) return null;

            applicant.ResumeUrl = updateDto.ResumeUrl;
            applicant.Experience = updateDto.Experience;
            applicant.PreferredLocation = updateDto.PreferredLocation;

            await _applicantRepository.UpdateApplicantAsync(applicant);
            await TriggerMatchSyncAsync(userId);

            return await GetProfileAsync(userId);

        }
        public async Task AddSkillAsync(int userId, int skillId)
        {
            var applicant = await _applicantRepository.GetProfileAsync(userId);
            if (applicant == null) return;

            await _applicantRepository.AddSkillAsync(applicant.ApplicantId, skillId);
            await _matchingService.TriggerApplicantMatchAsync(applicant.ApplicantId);
        }
        public async Task RemoveSkillAsync(int userId, int skillId)
        {
            var applicant = await _applicantRepository.GetProfileAsync(userId);
            if (applicant == null) return;

            await _applicantRepository.RemoveSkillAsync(applicant.ApplicantId, skillId);
            await TriggerMatchSyncAsync(userId);
        }
        public async Task TriggerMatchSyncAsync(int userId)
        {
            var applicant = await _applicantRepository.GetProfileAsync(userId);
            if (applicant == null) return;

            try
            {
                await _matchingService.TriggerApplicantMatchAsync(applicant.ApplicantId);
            }
            catch
            {
                // ML faliure should not block
            }
        }

    }
}