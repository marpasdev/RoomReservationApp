using RoomReservationSystem.Shared.Enums;

namespace RoomReservationSystem.Shared.Models
{
    public class ReservationHistory
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime ChangedAt { get; set; }
        public ReservationStatus OldStatus { get; set; }
        public ReservationStatus NewStatus { get; set; }
        public int ChangedByUserId { get; set; }
    }
}
