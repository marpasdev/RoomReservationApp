using RoomReservationSystem.Shared.DTOs.Users;
using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Shared.DTOs.Auth
{
    public class LoginResponse
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public UserDto User { get; set; } = new();
    }
}
