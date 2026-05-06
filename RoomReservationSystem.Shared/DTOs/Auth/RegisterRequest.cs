using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Shared.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string PasswordConfirmation { get; set; } = string.Empty;
    }
}
