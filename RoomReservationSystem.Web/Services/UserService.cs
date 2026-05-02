using RoomReservationSystem.Shared.DTOs.Users;
using RoomReservationSystem.Web.Repositories;
using RoomReservationSystem.Shared.Models;

namespace RoomReservationSystem.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ChangePasswordAsync(UpdatePasswordRequest request, int userId)
        {
            User? user = await userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new Exception("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
            {
                throw new Exception("Passwords do not match.");
            }

            if (request.NewPassword != request.NewPasswordConfirmation)
            {
                throw new Exception("Password confirmation does not match.");
            }

            await userRepository.UpdateAsync(new User()
            {
                Id = userId,
                UserName = user.UserName,
                CreatedAt = user.CreatedAt,
                Email = user.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword)
            });
        }

        public async Task DeleteAsync(int id)
        {
            await userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            IEnumerable<User> users = await userRepository.GetAllAsync();
            return users.Select(u => new UserDto()
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            });
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            User? user = await userRepository.GetByIdAsync(id);
            if (user is null) { return null; }

            return new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task UpdateAsync(UpdateUserRequest request)
        {
            User? user = await userRepository.GetByIdAsync(request.Id);

            if (user is null)
            {
                throw new Exception("User not found.");
            }

            await userRepository.UpdateAsync(new User()
            {
                Id = request.Id,
                Email = request.Email,
                UserName = request.UserName,
                PasswordHash = user.PasswordHash
            });
        }
    }
}
