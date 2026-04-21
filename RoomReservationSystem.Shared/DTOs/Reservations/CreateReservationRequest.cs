namespace RoomReservationSystem.Shared.DTOs.Reservations
{
    public class CreateReservationRequest
    {
        public int RoomId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public int PersonCount { get; set; }
        public int BookerId { get; set; }
    }
}
