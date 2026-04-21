using RoomReservationSystem.Shared.DTOs.Equipment;

namespace RoomReservationSystem.Shared.DTOs.Rooms
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public required List<EquipmentDto> Equipment { get; set; } 
        public int MaxReservationMinutes { get; set; }
    }
}
