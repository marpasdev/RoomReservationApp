namespace RoomReservationSystem.Shared.DTOs.Users
{
    public class UpdatePasswordRequest
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string NewPasswordConfirmation { get; set; } = string.Empty;
    }
}
