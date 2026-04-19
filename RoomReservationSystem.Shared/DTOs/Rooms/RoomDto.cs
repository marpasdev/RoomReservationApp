using RoomReservationSystem.Shared.DTOs.Equipment;

namespace RoomReservationSystem.Shared.DTOs.Rooms
{
    public class RoomDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Capacity { get; set; }
        public List<EquipmentDto> Equipment { get; set; } 
        public TimeSpan MaxReservationTime { get; set; }
    }
}
