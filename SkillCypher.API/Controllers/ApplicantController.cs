using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCypher.Core.DTOs.Applicant;
using SkillCypher.Core.Interfaces;

namespace SkillCypher.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicantService;
        public ApplicantController (IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var profile = await _applicantService.GetProfileAsync(userId);
            
            if(profile == null)
            {
                return NotFound(new
                {
                    message = "Applicant profile not found."
                });
            }
            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> Updateprofile([FromBody] UpdateApplicantProfileDto updateDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var updatedProfile = await _applicantService.UpdateProfileAsync(userId, updateDto);
            if(updatedProfile == null)
            {
                return NotFound(new
                {
                    message = "Applicant profile not found."
                });
            }
            return Ok(updatedProfile);
        }

        [HttpPost("skills")]
        public async Task<IActionResult> AddSkill([FromBody] int skillId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _applicantService.AddSkillAsync(userId,skillId);
            return Ok(new
            {
                message = "Skill added successfully."
            });
        }
        [HttpDelete("skills/{skillId}")]
        public async Task<IActionResult> RemoveSkill(int skillId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _applicantService.RemoveSkillAsync(userId, skillId);
            return Ok(new
            {
                message = "Skill removed successfully."
            });
        }
    }
}