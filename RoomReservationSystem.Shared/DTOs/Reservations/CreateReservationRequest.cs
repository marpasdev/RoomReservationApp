using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Shared.DTOs.Reservations
{
    public class CreateReservationRequest
    {
        public int RoomId { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        public string Purpose { get; set; } = string.Empty;
        [Range(1, 100)]
        public int PersonCount { get; set; }
        public int BookerId { get; set; }
    }
}
