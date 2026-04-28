using Microsoft.EntityFrameworkCore;
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
    }
}