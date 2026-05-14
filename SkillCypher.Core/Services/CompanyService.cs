using SkillCypher.Core.DTOs.Company;
using SkillCypher.Core.Interfaces;
using SkillCypher.Core.Models;

namespace SkillCypher.Core.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyResponseDto> CreateCompanyAsync(CreateCompanyDto dto)
        {
            var company = new Company
            {
                CompanyName = dto.CompanyName,
                CompanyDescription = dto.CompanyDescription,
                Website = dto.Website,
                Address = dto.Address,
                Industry = dto.Industry
            };

            var createdCompany = await _companyRepository.CreateCompanyAsync(company);

            return new CompanyResponseDto
            {
                CompanyId = createdCompany.CompanyId,
                CompanyName = createdCompany.CompanyName,
                CompanyDescription = createdCompany.CompanyDescription,
                Website = createdCompany.Website,
                Address = createdCompany.Address,
                Industry = createdCompany.Industry
            };
        }

        public async Task<CompanyResponseDto?> GetCompanyByIdAsync(int companyId)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(companyId);
            if(company == null)
                return null;

            return new CompanyResponseDto
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                CompanyDescription = company.CompanyDescription,
                Website = company.Website,
                Address = company.Address,
                Industry = company.Industry
            };
        }
        
        public async Task<CompanyResponseDto?> UpdateCompanyAsync(int companyId,CreateCompanyDto dto)
        {
            var company = new Company
            {
                CompanyId = companyId,
                CompanyName = dto.CompanyName,
                CompanyDescription = dto.CompanyDescription,
                Website = dto.Website,
                Address = dto.Address,
                Industry = dto.Industry
            };

            var updatedCompany = await _companyRepository
                .UpdateCompanyAsync(company);
            if(updatedCompany == null) return null;

            return new CompanyResponseDto
            {
                CompanyId = updatedCompany.CompanyId,
                CompanyName = updatedCompany.CompanyName,
                CompanyDescription = updatedCompany.CompanyDescription,
                Industry = updatedCompany.Industry,
                Address = updatedCompany.Address,
                Website = updatedCompany.Website
            };
        }

        public async Task AssignRecruiterAsync(int recruiterId,int companyId)
        {
            await _companyRepository.AssignRecruiterAsync(recruiterId, companyId);
        }

    }
}