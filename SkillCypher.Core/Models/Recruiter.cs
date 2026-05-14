
namespace SkillCypher.Core.Models
{

    public class Recruiter
    {
        public int RecruiterId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
        public string? Designation { get; set; }

    }
}