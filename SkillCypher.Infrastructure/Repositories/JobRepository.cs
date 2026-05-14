using Microsoft.EntityFrameworkCore;
using SkillCypher.Core.DTOs.Job;
using SkillCypher.Core.Interfaces;
using SkillCypher.Core.Models;
using SkillCypher.Infrastructure.Data;

namespace SkillCypher.Infrastructure.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _context;
        public JobRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Job> CreateJobAsync(Job job)
        {
            if (job.CompanyId == 0)
            {
                var recruiterCompanyId = await _context.Recruiters
                    .Where(r => r.RecruiterId == job.RecruiterId)
                    .Select(r => r.CompanyId)
                    .FirstOrDefaultAsync();

                if (!recruiterCompanyId.HasValue)
                {
                    throw new InvalidOperationException("Recruiter does not have a company assigned.");
                }

                job.CompanyId = recruiterCompanyId.Value;
            }

            if (job.JobSkills != null)
            {
                foreach (var jobSkill in job.JobSkills)
                {
                    jobSkill.Job = job;
                }
            }

            if (job.JobCertificates != null)
            {
                foreach (var jobCertificate in job.JobCertificates)
                {
                    jobCertificate.Job = job;
                }
            }

            await _context.Jobs.AddAsync(job);

            await _context.SaveChangesAsync();

            return job;
        }

        public async Task<bool> DeleteJobAsync(int jobId)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.JobId == jobId);
            if (job == null)
            {
                return false;
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Job?> GetJobByIdAsync(int jobId)
        {
            return await _context.Jobs
                .Include(j => j.Company)
                .Include(j => j.JobSkills)
                    .ThenInclude(js => js.Skill)
                .Include(j => j.JobCertificates)
                    .ThenInclude(jc => jc.Certificate)
                .FirstOrDefaultAsync(j => j.JobId == jobId);
        }

        public async Task<IEnumerable<Job>> GetJobsByRecruiterIdAsync(int recruiterId)
        {
            return await _context.Jobs
                .AsNoTracking()
                .Include(j => j.Company)
                .Where(j => j.RecruiterId == recruiterId)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Job> Jobs, int TotalCount)> GetJobsAsync(JobQueryParams queryParams)
        {
            var query = _context.Jobs
                .AsNoTracking()
                .Include(j => j.Company)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                query = query.Where(j => j.Title != null && j.Title.Contains(queryParams.Search));
            }

            if (!string.IsNullOrWhiteSpace(queryParams.Location))
            {
                query = query.Where(j => j.Location != null && j.Location.Contains(queryParams.Location));
            }

            if (queryParams.MinSalary.HasValue)
            {
                query = query.Where(j => j.MaxSalary >= queryParams.MinSalary);
            }

            if (queryParams.MaxSalary.HasValue)
            {
                query = query.Where(j => j.MinSalary <= queryParams.MaxSalary);
            }

            var totalCount = await query.CountAsync();
            var jobs = await query
                .OrderByDescending(j => j.PostedAt)
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return (jobs, totalCount);
        }

        public async Task<Recruiter?> GetRecruiterIdByUserIdAsync(int userId)
        {
            return await _context.Recruiters.FirstOrDefaultAsync(r => r.UserId == userId);
        }

        public async Task<Job?> UpdateJobAsync(Job job)
        {
            var existingJob = await _context.Jobs
                .Include(j => j.JobSkills)
                .Include(j => j.JobCertificates)
                .FirstOrDefaultAsync(j => j.JobId == job.JobId);
            if (existingJob == null)
            {
                return null;
            }

            existingJob.Title = job.Title;
            existingJob.Description = job.Description;
            existingJob.Location = job.Location;
            existingJob.Requirements = job.Requirements;
            existingJob.JobType = job.JobType;
            existingJob.MinSalary = job.MinSalary;
            existingJob.MaxSalary = job.MaxSalary;
            existingJob.CompanyId = job.CompanyId;

            _context.JobSkills.RemoveRange(existingJob.JobSkills);
            _context.JobCertificates.RemoveRange(existingJob.JobCertificates);

            existingJob.JobSkills = job.JobSkills?.ToList() ?? new List<JobSkill>();
            existingJob.JobCertificates = job.JobCertificates?.ToList() ?? new List<JobCertificate>();

            if (existingJob.JobSkills.Any())
            {
                await _context.JobSkills.AddRangeAsync(existingJob.JobSkills);
            }

            if (existingJob.JobCertificates.Any())
            {
                await _context.JobCertificates.AddRangeAsync(existingJob.JobCertificates);
            }

            await _context.SaveChangesAsync();
            return await _context.Jobs
                .Include(j => j.Company)
                .Include(j => j.JobSkills)
                    .ThenInclude(js => js.Skill)
                .Include(j => j.JobCertificates)
                    .ThenInclude(jc => jc.Certificate)
                .FirstOrDefaultAsync(j => j.JobId == job.JobId);
        }
    }
}