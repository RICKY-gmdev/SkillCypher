using SkillCypher.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using SkillCypher.Infrastructure.Data;
using SkillCypher.Core.Models;

namespace SkillCypher.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task CreateApplicantProfileAsync(int userId)
        {
            var applicant = new Applicant
            {
                UserId = userId
            };
            await _context.Applicants.AddAsync(applicant);
            await _context.SaveChangesAsync();
        }
    }
}