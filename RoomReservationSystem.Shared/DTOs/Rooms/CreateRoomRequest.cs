namespace RoomReservationSystem.Shared.DTOs.Rooms
{
    public class CreateRoomRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public List<RoomEquipmentRequest> Equipment { get; set; } = new();
        public int MaxReservationMinutes { get; set; }
    }
}
