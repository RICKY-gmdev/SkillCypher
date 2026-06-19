using SkillCypher.Core.Models;

namespace SkillCypher.Core.Interfaces
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> GetAllSkillsAsync();
    }
}