using Microsoft.AspNetCore.Mvc;
using SkillCypher.Core.Interfaces;

namespace SkillCypher.API.Controllers
{
    [ApiController]
    [Route("api/match")]
    public class MatchController : ControllerBase
    {
        private readonly IJobMatchRepository _jobMatchRepository;
        public MatchController(IJobMatchRepository jobMatchRepository)
        {
            _jobMatchRepository = jobMatchRepository;
        }

        [HttpGet("{applicantId:int}/{jobId:int}")]
        public async Task<IActionResult> GetMatch(int applicantId,int jobId)
        {
            var match = await _jobMatchRepository.GetJobMatchAsync(applicantId,jobId);
            if(match == null)
            {
                return NotFound(new {message = "Match not found."});
            }
            return Ok(new
            {
                applicantId = match.ApplicantId,
                jobId = match.JobId,
                matchScore = match.MatchScore
            });
        }
    }
}