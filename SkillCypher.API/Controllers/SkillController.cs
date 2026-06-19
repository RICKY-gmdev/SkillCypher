using Microsoft.AspNetCore.Mvc;
using SkillCypher.Core.Interfaces;

namespace SkillCypher.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillRepository _repo;
        public SkillsController( ISkillRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetSkills()
        {
            var skills = await _repo.GetAllSkillsAsync();
            return Ok(skills);
        }
    }
    
}