using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCypher.Core.DTOs.Job;
using SkillCypher.Core.Interfaces;
using SkillCypher.Core.Models;

namespace SkillCypher.API.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobs([FromQuery] JobQueryParams queryParams)
        {
            var (jobs, totalCount) = await _jobService.GetJobsAsync(queryParams);
            return Ok(new
            {
                TotalCount = totalCount,
                Jobs = jobs
            });
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetJobById(int jobId)
        {
            var job = await _jobService.GetJobByIdAsync(jobId);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [HttpPost]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobDto dto)
        {
            try
            {
                var useridClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (useridClaim == null)
                    return Unauthorized();

                if (!int.TryParse(useridClaim.Value, out var userId))
                    return Unauthorized();

                var recruiterId = await _jobService.GetRecruiterIdByuserIdAsync(userId);

                if (recruiterId == null)
                    return Unauthorized();

                var created = await _jobService.CreateJobAsync(dto, recruiterId.Value);

                return CreatedAtAction(nameof(GetJobById), new { jobId = created.JobId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [HttpPut("{jobId}")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> UpdateJob(int jobId, [FromBody] CreateJobDto dto)
        {
            var useridClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (useridClaim == null)
                return Unauthorized();

            if (!int.TryParse(useridClaim.Value, out var userId))
                return Unauthorized();
            var recruiterId = await _jobService.GetRecruiterIdByuserIdAsync(userId);

            if (recruiterId == null)
                return Unauthorized();

            var updated = await _jobService.UpdateJobAsync(jobId, dto, recruiterId.Value);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{jobId}")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> DeleteJob(int jobId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();
            var recruiterId = await _jobService.GetRecruiterIdByuserIdAsync(userId);

            if (recruiterId == null)
                return Unauthorized();

            try
            {
                var ok = await _jobService.DeleteJobAsync(jobId, recruiterId.Value);
                if (!ok) return NotFound();
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("recruiter")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> GetRecruiterJobs()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();
            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();
            var recruiterId = await _jobService.GetRecruiterIdByuserIdAsync(userId);

            if (recruiterId == null)
                return Unauthorized();
            var jobs = await _jobService.GetJobsByRecruiterIdAsync(recruiterId.Value);

            return Ok(jobs);
        }
    }
}