using Microsoft.EntityFrameworkCore;
using SkillCypher.Core.Interfaces;
using SkillCypher.Core.Models;
using SkillCypher.Infrastructure.Data;

namespace SkillCypher.Infrastructure.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;
        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Application> ApplyToJobAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<Application?> GetApplicationByIdAsync(int applicationId)
        {
            return await _context.Applications
                .Include(a => a.Applicant)
                .ThenInclude(applicant => applicant.User)
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);
        }

        public async Task<IEnumerable<Application>> GetApplicationByJobIdAsync(int jobId)
        {
            return await _context.Applications
                .Include(a => a.Applicant)
                .ThenInclude(applicant => applicant.User)
                .Where(a => a.JobId == jobId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationByApplicantIdAsync(int applicantId)
        {
            return await _context.Applications
                .Include(a => a.Applicant)
                .ThenInclude(applicant => applicant.User)
                .Where(a => a.ApplicantId == applicantId)
                .ToListAsync();
        }

        public async Task WithdrawApplicationAsync(int applicationId, int applicantId)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(a =>
                a.ApplicationId == applicationId &&
                a.ApplicantId == applicantId);

            if(application == null)
            {
                return;
            }

            application.Status = ApplicationStatus.Withdrawn;
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasApplicantAppliedAsync(int applicantId, int jobId)
        {
            return await _context.Applications.AnyAsync(a =>
            a.ApplicantId == applicantId &&
            a.JobId == jobId &&
            a.Status != ApplicationStatus.Withdrawn
            );
        }
    }
}