using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCypher.Core.DTOs.Company;
using SkillCypher.Core.Interfaces;

namespace SkillCypher.API.Controllers
{

    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IJobRepository _jobRepository;

        public CompanyController(ICompanyService companyService, IJobRepository jobRepository)
        {
            _companyService = companyService;
            _jobRepository = jobRepository;
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid token."
                });
            }
            int userId = int.Parse(userIdClaim);

            var createdCompany = await _companyService.CreateCompanyAsync(dto);

            var recruiter = await _jobRepository.GetRecruiterIdByUserIdAsync(userId);
            if (recruiter == null) return Unauthorized();
            await _companyService.AssignRecruiterAsync(recruiter.RecruiterId, createdCompany.CompanyId);

            return CreatedAtAction("GetCompanyById", new { id = createdCompany.CompanyId }, createdCompany);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound(new
                {
                    message = "Company not found."
                });
            }

            return Ok(company);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CreateCompanyDto dto)
        {
            var updatedCompany = await _companyService.UpdateCompanyAsync(id, dto);
            if (updatedCompany == null)
            {
                return NotFound(new
                {
                    message = "Company not found."
                });
            }

            return Ok(updatedCompany);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignRecruiter(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid token."
                });
            }

            int userId = int.Parse(userIdClaim);

            var recruiterId = await _jobRepository.GetRecruiterIdByUserIdAsync(userId);

            if (recruiterId == null)
            {
                return NotFound(new
                {
                    message = "Recruiter profile not found."
                });
            }

            var company = await _companyService.GetCompanyByIdAsync(id);

            if (company == null)
            {
                return NotFound(new
                {
                    message = "Company not found."
                });
            }
            await _companyService.AssignRecruiterAsync(recruiterId.RecruiterId, id);

            return Ok(new
            {
                message = "Recruiter assigned successfully."
            });
        }
    }
}