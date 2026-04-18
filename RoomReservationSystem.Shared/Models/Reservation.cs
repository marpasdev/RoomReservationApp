using RoomReservationSystem.Shared.Enums;

namespace RoomReservationSystem.Shared.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public required string Purpose { get; set; }
        public int PersonCount { get; set; }
        public ReservationStatus Status { get; set; }
        public int BookerUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
