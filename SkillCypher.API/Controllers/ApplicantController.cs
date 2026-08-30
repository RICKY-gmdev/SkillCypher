using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCypher.Core.DTOs;
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

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var dashboard = await _applicantService.GetDashboardDtoAsync(userId);
            if(dashboard == null)
                return NotFound(new { message = "Dashboard data not found"});
            return Ok(dashboard);
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
        public async Task<IActionResult> AddSkill([FromBody] AddSkillDto dto)
        {
            var skillId = dto.SkillId;
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _applicantService.AddSkillAsync(userId,skillId);
            return Ok(new
            {
                message = "Skill added successfully."
            });
        }
        [HttpPost("skills/sync")]
        public async Task<IActionResult> SyncSkills()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _applicantService.TriggerMatchSyncAsync(userId);
            return Ok();
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