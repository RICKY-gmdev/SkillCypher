using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCypher.Core.DTOs.Application;
using SkillCypher.Core.Interfaces;

namespace SkillCypher.API.Controllers
{
    [ApiController]
    [Route("api/applications")]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IJobService _jobService;

        public ApplicationController(
            IApplicationService applicationService,
            IApplicantRepository applicantRepository,
            IJobService jobService)
        {
            _applicationService = applicationService;
            _applicantRepository = applicantRepository;
            _jobService = jobService;
        }

        [HttpPost]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> ApplyToJob([FromBody] CreateApplicationDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var applicant = await _applicantRepository.GetProfileAsync(userId);
            if (applicant == null)
            {
                return NotFound(new { message = "Applicant profile not found." });
            }

            try
            {
                var createdApplication = await _applicationService.ApplyToJobAsync(applicant.ApplicantId, dto);
                return Ok(createdApplication);
            }
            catch (Exception ex) when (ex.Message == "Applicant has already applied for this job.")
            {
                return Conflict(new { message = ex.Message });
            }
            
        }

        [HttpDelete("{applicationId:int}/withdraw")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> WithdrawApplication(int applicationId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var applicant = await _applicantRepository.GetProfileAsync(userId);
            if (applicant == null)
            {
                return NotFound(new { message = "Applicant profile not found." });
            }

            try
            {
                await _applicationService.WithdrawApplicationAsync(applicationId, applicant.ApplicantId);
                return NoContent();
            }
            catch (Exception ex) when (ex.Message == "Application not found.")
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex) when (ex.Message == "Unauthorized withdrawal attempt.")
            {
                return Forbid();
            }
        }

        [HttpGet("my")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> GetMyApplications()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var applicant = await _applicantRepository.GetProfileAsync(userId);
            if (applicant == null)
            {
                return NotFound(new { message = "Applicant profile not found." });
            }

            var applications = await _applicationService.GetApplicationByApplicantIdAsync(applicant.ApplicantId);
            return Ok(applications);
        }

        [HttpGet("/api/jobs/{jobId:int}/applications")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> GetApplicationsForJob(int jobId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var recruiterId = await _jobService.GetRecruiterIdByuserIdAsync(userId);
            if (recruiterId == null)
            {
                return Unauthorized();
            }

            var job = await _jobService.GetJobByIdAsync(jobId);
            if (job == null)
            {
                return NotFound(new { message = "Job not found." });
            }

            if (job.RecruiterId != recruiterId.Value)
            {
                return Forbid();
            }

            var applications = await _applicationService.GetApplicationByJobIdAsync(jobId);
            return Ok(applications);
        }
    }
}