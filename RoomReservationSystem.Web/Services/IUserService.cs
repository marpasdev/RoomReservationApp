using RoomReservationSystem.Shared.DTOs.Users;

namespace RoomReservationSystem.Web.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task UpdateAsync(UpdateUserRequest request);
        Task DeleteAsync(int id);
        Task ChangePasswordAsync(UpdatePasswordRequest request, int userId);
    }
}
