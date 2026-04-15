namespace SkillCypher.Core.Models
{
public class Company
{
    public int CompanyId {get;set;}
    public string CompanyName {get;set;} = string.Empty;
    public string CompanyDescription {get;set;} = string.Empty;
    public string Industry{get;set;} = string.Empty;
    public string Address {get;set;} = string.Empty;
    public string? Website{get;set;}
}
}