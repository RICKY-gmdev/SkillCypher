using SkillCypher.Core.DTOs.Auth;
using SkillCypher.Core.Interfaces;
using SkillCypher.Core.Models;


namespace SkillCypher.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtService _jwtService;
        public AuthService(IAuthRepository authRepository, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
        }
        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            bool emailExists = await _authRepository.EmailExistsAsync(registerDto.Email);

            if(emailExists)
            {
                return null;
            }

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = registerDto.Role,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _authRepository.CreateUserAsync(user);

            if(createdUser.Role == UserRole.Applicant)
            {
                await _authRepository.CreateApplicantProfileAsync(createdUser.UserId);
            }
            
            string token = _jwtService.GenerateToken(createdUser);

            return new AuthResponseDto
            {
                Role = createdUser.Role,
                UserId = createdUser.UserId,
                Name = createdUser.Name,
                Email = createdUser.Email,
                Token = token
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _authRepository.GetUserByEmailAsync(loginDto.Email);

            if(user == null) return null;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                loginDto.Password,
                user.PasswordHash
            );

            if(!isPasswordValid)
            {
                return null;
            }

            string token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }

        
    }
}