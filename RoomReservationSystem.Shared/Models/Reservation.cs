using RoomReservationSystem.Shared.Enums;

namespace RoomReservationSystem.Shared.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public int PersonCount { get; set; }
        public ReservationStatus Status { get; set; }
        public int BookerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
