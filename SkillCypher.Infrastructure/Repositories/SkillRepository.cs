using Microsoft.EntityFrameworkCore;
using SkillCypher.Core.Interfaces;
using SkillCypher.Core.Models;
using SkillCypher.Infrastructure.Data;

namespace SkillCypher.Infrastructure.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly AppDbContext _context;
        public SkillRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
        {
            return await _context.Skills
                .OrderBy(s => s.SkillName)
                .ToListAsync();
        }
    }
}