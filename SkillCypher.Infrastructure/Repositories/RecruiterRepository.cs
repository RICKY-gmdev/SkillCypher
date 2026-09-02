using Microsoft.EntityFrameworkCore;
using SkillCypher.Core.DTOs.Recruiter;
using SkillCypher.Core.Interfaces;
using SkillCypher.Infrastructure.Data;

namespace SkillCypher.Infrastructure.Repositories
{
    public class RecruiterRepository : IRecruiterRepository
    {
        private readonly AppDbContext _context;

        public RecruiterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RecruiterDashboardDto?> GetDashboardAsync(int userId)
        {
            var recruiter = await _context.Recruiters
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.UserId == userId);

            if (recruiter == null)
                return null;

            var jobs = await _context.Jobs
                .AsNoTracking()
                .Where(j => j.RecruiterId == recruiter.RecruiterId)
                .Select(j => new
                {
                    j.JobId,
                    j.Title
                })
                .ToListAsync();

            var jobIds = jobs
                .Select(j => j.JobId)
                .ToList();

            var applications = await _context.Applications
                .AsNoTracking()
                .Where(a => jobIds.Contains(a.JobId))
                .Select(a => new
                {
                    a.JobId,
                    a.ApplicantId,
                    ApplicantName = a.Applicant.User.Name
                })
                .ToListAsync();

            var matches = await _context.JobMatches
                .AsNoTracking()
                .Where(jm => jobIds.Contains(jm.JobId))
                .Select(jm => new
                {
                    jm.JobId,
                    jm.ApplicantId,
                    jm.MatchScore
                })
                .ToListAsync();

            var jobDtos = jobs.Select(job =>
            {
                var jobApplications = applications
                    .Where(a => a.JobId == job.JobId)
                    .Select(a =>
                    {
                        var match = matches.FirstOrDefault(m =>
                            m.JobId == job.JobId &&
                            m.ApplicantId == a.ApplicantId);

                        return new RecruiterApplicationMatchDto
                        {
                            ApplicantId = a.ApplicantId,
                            ApplicantName = a.ApplicantName ?? string.Empty,
                            MatchScore = match != null
                                ? (double)match.MatchScore
                                : 0
                        };
                    })
                    .OrderByDescending(a => a.MatchScore)
                    .ToList();

                return new RecruiterJobDashboardDto
                {
                    JobId = job.JobId,
                    Title = job.Title ?? string.Empty,
                    TotalApplicants = jobApplications.Count,
                    TopMatchScore = jobApplications.Count > 0
                        ? jobApplications.Max(a => a.MatchScore)
                        : 0,
                    Applicants = jobApplications
                };
            }).ToList();

            return new RecruiterDashboardDto
            {
                RecruiterName = recruiter.User.Name ?? string.Empty,
                TotalJobsPosted = jobDtos.Count,
                TotalApplicationsReceived = applications.Count,
                Jobs = jobDtos
            };
        }
    }
}