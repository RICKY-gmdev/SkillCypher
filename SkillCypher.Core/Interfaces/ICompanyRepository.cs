using SkillCypher.Core.Models;

namespace SkillCypher.Core.Interfaces
{
    public interface ICompanyRepository{

    
    Task<Company> CreateCompanyAsync(Company company);
    Task<Company?> GetCompanyByIdAsync(int companyId);
    Task<Company?> UpdateCompanyAsync(Company company);
    Task AssignRecruiterAsync(int recruiterId,int companyId);
    }
}