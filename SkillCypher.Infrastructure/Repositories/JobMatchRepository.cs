using Microsoft.EntityFrameworkCore;
using SkillCypher.Core.Interfaces;
using SkillCypher.Core.Models;
using SkillCypher.Infrastructure.Data;

namespace SkillCypher.Infrastructure.Repositories
{
    public class JobMatchRepository : IJobMatchRepository
    {
        private readonly AppDbContext _db;
        public JobMatchRepository(AppDbContext db)
        {
            _db = db;
        }
        public Task<JobMatch?> GetJobMatchAsync(int applicantId, int jobId)
        {
            return _db.JobMatches.FirstOrDefaultAsync(x => x.ApplicantId == applicantId && x.JobId == jobId);
        }
    }
}