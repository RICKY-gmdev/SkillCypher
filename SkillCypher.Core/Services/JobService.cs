using SkillCypher.Core.DTOs.Applicant;
using SkillCypher.Core.DTOs.Job;
using SkillCypher.Core.Interfaces;
using SkillCypher.Core.Models;


namespace SkillCypher.Core.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<JobResponseDto> CreateJobAsync(CreateJobDto createJobDto, int recruiterId)
        {
            var distinctSkillIds = createJobDto.SkillIds.Distinct().ToList();
            var distinctCertificateIds = createJobDto.CertificateIds.Distinct().ToList();

            var job = new Job
            {
                Title = createJobDto.Title,
                Description = createJobDto.Description,
                JobType = createJobDto.JobType,
                Location = createJobDto.Location,
                Requirements = createJobDto.Requirements,
                MinSalary = createJobDto.MinSalary,
                MaxSalary = createJobDto.MaxSalary,
                CompanyId = createJobDto.CompanyId,
                PostedAt = DateTime.UtcNow,
                RecruiterId = recruiterId
            };
            job.JobSkills = distinctSkillIds
                .Select(skillId => new JobSkill
                {
                    SkillId = skillId
                }).ToList();

            job.JobCertificates = distinctCertificateIds
                .Select(certId => new JobCertificate
                {
                    CertificateId = certId
                }).ToList();

            var createdJob = await _jobRepository.CreateJobAsync(job);
            var createdJobWithDetails = await _jobRepository.GetJobByIdAsync(createdJob.JobId);
            if (createdJobWithDetails == null)
            {
                return new JobResponseDto
                {
                    JobId = createdJob.JobId,
                    Title = createdJob.Title ?? string.Empty,
                    Description = createdJob.Description ?? string.Empty,
                    MinSalary = createdJob.MinSalary,
                    MaxSalary = createdJob.MaxSalary,
                    Location = createdJob.Location ?? string.Empty,
                    JobType = createdJob.JobType ?? string.Empty,
                    Requirements = createdJob.Requirements ?? string.Empty,
                    RecruiterId = createdJob.RecruiterId,
                    CompanyId = createdJob.CompanyId,
                    CompanyName = createdJob.Company?.CompanyName ?? string.Empty,
                    PostedAt = createdJob.PostedAt ?? DateTime.UtcNow
                };
            }

            return MapToJobResponseDto(createdJobWithDetails);
        }

        public async Task<bool> DeleteJobAsync(int jobId, int recruiterId)
        {
            var existingJob = await _jobRepository.GetJobByIdAsync(jobId);

            if (existingJob == null)
                return false;

            if (existingJob.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException("You can only delete your own jobs.");
            }

            return await _jobRepository.DeleteJobAsync(jobId);
        }

        public async Task<JobResponseDto?> GetJobByIdAsync(int jobId)
        {
            var job = await _jobRepository.GetJobByIdAsync(jobId);
            if (job == null) return null;
            return new JobResponseDto
            {
                JobId = job.JobId,
                Title = job.Title ?? string.Empty,
                Description = job.Description ?? string.Empty,
                Location = job.Location ?? string.Empty,
                JobType = job.JobType ?? string.Empty,
                Requirements = job.Requirements ?? string.Empty,
                RecruiterId = job.RecruiterId,
                PostedAt = job.PostedAt ?? DateTime.UtcNow,
                Skills = job.JobSkills?
                    .Select(s => new SkillDto
                    {
                        SkillId = s.SkillId,
                        SkillName = s.Skill.SkillName
                    })
                    .ToList() ?? new List<SkillDto>(),
                Certificates = job.JobCertificates?
                    .Select(c => new CertificateDto
                    {
                        CertificateId = c.CertificateId,
                        CertificateName = c.Certificate.CertificateName,
                        IssuedBy = c.Certificate.IssuingBody
                    })
                    .ToList() ?? new List<CertificateDto>(),
                CompanyId = job.CompanyId,
                CompanyName = job.Company?.CompanyName ?? string.Empty
            };
        }

        public async Task<IEnumerable<JobListItemDto>> GetJobsByRecruiterIdAsync(int recruiterId)
        {
            var jobs = await _jobRepository.GetJobsByRecruiterIdAsync(recruiterId);
            return jobs.Select(MapToJobListItemDto);
        }

        public async Task<(IEnumerable<JobListItemDto> Jobs, int TotalCount)> GetJobsAsync(JobQueryParams queryParams)
        {
            var (jobs, totalCount) = await _jobRepository.GetJobsAsync(queryParams);
            return (jobs.Select(MapToJobListItemDto), totalCount);
        }

        public async Task<JobResponseDto?> UpdateJobAsync(int jobId, CreateJobDto updateJobDto, int recruiterId)
        {
            var existingJob = await _jobRepository.GetJobByIdAsync(jobId);
            if (existingJob == null) return null;
            if (existingJob.RecruiterId != recruiterId)
                throw new UnauthorizedAccessException("You are not Authorized to edit this job.");

            var distinctSkillIds = updateJobDto.SkillIds.Distinct().ToList();
            var distinctCertificateIds = updateJobDto.CertificateIds.Distinct().ToList();

            existingJob.Title = updateJobDto.Title;
            existingJob.Description = updateJobDto.Description;
            existingJob.MinSalary = updateJobDto.MinSalary;
            existingJob.MaxSalary = updateJobDto.MaxSalary;
            existingJob.Location = updateJobDto.Location;
            existingJob.Requirements = updateJobDto.Requirements;
            existingJob.JobType = updateJobDto.JobType;
            existingJob.CompanyId = updateJobDto.CompanyId;

            existingJob.JobSkills = updateJobDto.SkillIds
                .Distinct()
                .Select(skillId => new JobSkill
                {
                    JobId = jobId,
                    SkillId = skillId
                })
                .ToList();

            existingJob.JobCertificates = distinctCertificateIds
                .Select(certId => new JobCertificate
                {
                    JobId = jobId,
                    CertificateId = certId
                })
                .ToList();

            var updatedJob = await _jobRepository.UpdateJobAsync(existingJob);

            if (updatedJob == null) return null;

            var updatedJobWithDetails = await _jobRepository.GetJobByIdAsync(updatedJob.JobId);
            return updatedJobWithDetails == null ? MapToJobResponseDto(updatedJob) : MapToJobResponseDto(updatedJobWithDetails);
        }

        private static JobListItemDto MapToJobListItemDto(Job job)
        {
            return new JobListItemDto
            {
                JobId = job.JobId,
                Title = job.Title ?? string.Empty,
                MinSalary = job.MinSalary,
                MaxSalary = job.MaxSalary,
                CompanyId = job.CompanyId,
                CompanyName = job.Company?.CompanyName ?? string.Empty
            };
        }

        private static JobResponseDto MapToJobResponseDto(Job job)
        {
            return new JobResponseDto
            {
                JobId = job.JobId,
                Title = job.Title ?? string.Empty,
                Description = job.Description ?? string.Empty,
                MinSalary = job.MinSalary,
                MaxSalary = job.MaxSalary,
                Location = job.Location ?? string.Empty,
                JobType = job.JobType ?? string.Empty,
                Requirements = job.Requirements ?? string.Empty,
                RecruiterId = job.RecruiterId,
                CompanyId = job.CompanyId,
                CompanyName = job.Company?.CompanyName ?? string.Empty,
                PostedAt = job.PostedAt ?? DateTime.UtcNow,
                Skills = job.JobSkills?
                    .Select(s => new SkillDto
                    {
                        SkillId = s.SkillId,
                        SkillName = s.Skill.SkillName
                    })
                    .ToList() ?? new List<SkillDto>(),
                Certificates = job.JobCertificates?
                    .Select(c => new CertificateDto
                    {
                        CertificateId = c.CertificateId,
                        CertificateName = c.Certificate.CertificateName,
                        IssuedBy = c.Certificate.IssuingBody
                    })
                    .ToList() ?? new List<CertificateDto>()
            };
        }

        public async Task<int?> GetRecruiterIdByuserIdAsync(int userId)
        {
            var recruiter = await _jobRepository.GetRecruiterIdByUserIdAsync(userId);
            if (recruiter == null)
                return null;
            return recruiter?.RecruiterId;
        }
    }
}