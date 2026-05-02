namespace RoomReservationSystem.Shared.DTOs.Rooms
{
    public class UpdateRoomRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public required IEnumerable<RoomEquipmentRequest> Equipment { get; set; }
        public int MaxReservationMinutes { get; set; }
    }
}
