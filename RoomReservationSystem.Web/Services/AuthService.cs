using BCrypt.Net;
using RoomReservationSystem.Shared.DTOs.Auth;
using RoomReservationSystem.Shared.DTOs.Users;
using RoomReservationSystem.Shared.Models;
using RoomReservationSystem.Web.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoomReservationSystem.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            User? user = await userRepository.GetByEmailAsync(request.Email);


            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null;
            }

            return new LoginResponse() { Token = GenerateToken(user), User = 
                new UserDto() { UserName = user.UserName, Email = user.Email,
                CreatedAt = user.CreatedAt, Id = user.Id }
            };
        }

        public async Task<int> RegisterAsync(RegisterRequest request)
        {
            if (request.Password != request.PasswordConfirmation)
            {
                throw new Exception("Password not equal to password confirmation");
            }
            if (await userRepository.GetByEmailAsync(request.Email) is not null)
            {
                throw new Exception("Email taken");
            }
            if (await userRepository.GetByUserNameAsync(request.UserName) is not null)
            {
                throw new Exception("Username taken");
            }

            return await userRepository.CreateAsync(new User()
            {
                UserName = request.UserName,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)    
            });
        }

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(configuration["Jwt:ExpiryMinutes"]!)),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
