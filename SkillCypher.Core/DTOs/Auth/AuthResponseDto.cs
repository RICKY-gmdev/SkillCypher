using SkillCypher.Core.Models;
namespace SkillCypher.Core.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string Name { get; set;} = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role {get;set;}
        public string Token { get; set; } = string.Empty;
    }
}