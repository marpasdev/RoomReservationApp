namespace RoomReservationSystem.Shared.DTOs.Reservations
{
    public class UpdateReservationRequest
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public int PersonCount { get; set; }
    }
}
