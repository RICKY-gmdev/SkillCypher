using SkillCypher.Core.Models;

namespace SkillCypher.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}