using SkillCypher.Core.Models;

namespace SkillCypher.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task CreateApplicantProfileAsync(int userId);
    }
}