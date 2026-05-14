using SkillCypher.Core.DTOs.Company;
using SkillCypher.Core.Models;

namespace SkillCypher.Core.Interfaces
{
    public interface ICompanyService{
    Task<CompanyResponseDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto);
    Task<CompanyResponseDto?> GetCompanyByIdAsync(int companyId);
    Task<CompanyResponseDto?> UpdateCompanyAsync(int companyId,CreateCompanyDto createCompanyDto);
    Task AssignRecruiterAsync(int recruiterId, int companyId);
    }
}