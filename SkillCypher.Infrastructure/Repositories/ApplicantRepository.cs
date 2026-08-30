using Microsoft.EntityFrameworkCore;
using SkillCypher.Core.DTOs.Applicant;
using SkillCypher.Core.Interfaces;
using SkillCypher.Core.Models;
using SkillCypher.Infrastructure.Data;

namespace SkillCypher.Infrastructure.Repositories
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly AppDbContext _context;
        public ApplicantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Applicant?> GetProfileAsync(int userId)
        {
            return await _context.Applicants
            .Include(a => a.User)
            .Include(a => a.ApplicantSkills)
                .ThenInclude(a => a.Skill)
            .Include(a => a.ApplicantCertificates)
                .ThenInclude(ac => ac.Certificate)
            .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<Applicant> UpdateApplicantAsync(Applicant applicant)
        {
            _context.Applicants.Update(applicant);
            await _context.SaveChangesAsync();
            return applicant;
        }

        public async Task AddSkillAsync(int applicantId, int skillId)
        {
            bool alreadyExists = await _context.ApplicantSkills
                .AnyAsync(a => 
                a.ApplicantId == applicantId&&
                a.SkillId == skillId);
            if(!alreadyExists)
            {
                await _context.ApplicantSkills.AddAsync(new ApplicantSkill
                {
                    ApplicantId = applicantId,
                    SkillId = skillId
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveSkillAsync(int applicantId, int skillId)
        {
            var applicantSkill = await _context.ApplicantSkills
                .FirstOrDefaultAsync(a => 
                a.ApplicantId == applicantId &&
                a.SkillId == skillId);
            if(applicantSkill != null)
            {
                _context.ApplicantSkills.Remove(applicantSkill);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<ApplicantDashboardDto> GetDashboardAsync(int userId)
        {
            var applicant = await _context.Applicants
            .Include(a => a.User)
            .Include(a => a.ApplicantSkills)
            .FirstOrDefaultAsync(applicant => applicant.UserId == userId);

            if (applicant == null)
            {
                return new ApplicantDashboardDto
                {
                    ApplicantName = string.Empty,
                    TotalSkills = 0,
                    TotalApplications = 0,
                    PreferredLocation = string.Empty,
                    JobMatches = new List<JobMatchSummaryDto>()
                };
            }
            var totalApplications = await _context.Applications.CountAsync(a => a.ApplicantId == applicant.ApplicantId);

            var JobMatches = await _context.JobMatches
                .Where(jm => jm.ApplicantId == applicant.ApplicantId)
                .Join(_context.Jobs,
                jm => jm.JobId,
                j => j.JobId,
                (jm, j ) => new{jm, j})
                .Join(_context.Companies,
                    x => x.j.CompanyId,
                    c => c.CompanyId,
                    (x,c) => new JobMatchSummaryDto
                    {
                        JobId = x.j.JobId,
                        Title = x.j.Title ?? string.Empty,
                        CompanyName =c.CompanyName ?? string.Empty,
                        Location = x.j.Location,
                        MatchScore = (double)x.jm.MatchScore
                    })
                .OrderByDescending(x => x.MatchScore)
                .ToListAsync();
            return new ApplicantDashboardDto
            {
                ApplicantName = applicant.User.Name ?? string.Empty,
                TotalSkills = applicant.ApplicantSkills.Count,
                TotalApplications = totalApplications,
                PreferredLocation = applicant.PreferredLocation,
                JobMatches = JobMatches
            };
        }
    }
}