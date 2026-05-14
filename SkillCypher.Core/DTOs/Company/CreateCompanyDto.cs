namespace SkillCypher.Core.DTOs.Company
{
    public class CreateCompanyDto
    {
        public string CompanyName{get;set;} = string.Empty;
        public string CompanyDescription{get;set;} =string.Empty;
        public string Industry {get;set;} = string.Empty;
        public string Address {get;set;} = string.Empty;
        public string? Website {get;set;}
    }
}