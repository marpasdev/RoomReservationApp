using RoomReservationSystem.Shared.DTOs.Auth;

namespace RoomReservationSystem.Web.Services
{
    public interface IAuthService
    {
        Task<int> RegisterAsync(RegisterRequest request);

        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
