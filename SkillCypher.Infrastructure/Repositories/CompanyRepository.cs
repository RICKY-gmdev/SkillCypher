using Microsoft.EntityFrameworkCore;
using SkillCypher.Core.Interfaces;
using SkillCypher.Core.Models;
using SkillCypher.Infrastructure.Data;

namespace SkillCypher.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AssignRecruiterAsync(int recruiterId, int companyId)
        {
            var recruiter = await _context.Recruiters
                .FirstOrDefaultAsync(r=>r.RecruiterId==recruiterId);

            if(recruiter == null)
                throw new Exception("Recruiter not found");

            recruiter.CompanyId = companyId;

            await _context.SaveChangesAsync();
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return company;
        }

        public async Task<Company?> GetCompanyByIdAsync(int companyId)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == companyId);
        }

        public async Task<Company?> UpdateCompanyAsync(Company company)
        {
            var existingCompany = await _context.Companies
                .FirstOrDefaultAsync(c=>c.CompanyId == company.CompanyId);
            if(existingCompany == null)
                return null;

            existingCompany.CompanyName = company.CompanyName;
            existingCompany.CompanyDescription = company.CompanyDescription;
            existingCompany.Website = company.Website;
            existingCompany.Address = company.Address;
            existingCompany.Industry = company.Industry;

            await _context.SaveChangesAsync();

            return existingCompany;
        }
    }
}