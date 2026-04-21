using RoomReservationSystem.Shared.DTOs.Users;

namespace RoomReservationSystem.Shared.DTOs.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new();
    }
}
