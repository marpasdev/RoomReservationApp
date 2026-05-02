using RoomReservationSystem.Shared.DTOs.Equipment;
using RoomReservationSystem.Shared.DTOs.Reservations;

namespace RoomReservationSystem.Shared.DTOs.Rooms
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public required IEnumerable<EquipmentDto> Equipment { get; set; } 
        public int MaxReservationMinutes { get; set; }
        public bool IsAvailable { get; set; }
        public ReservationDto? CurrentReservation { get; set; }
    }
}
