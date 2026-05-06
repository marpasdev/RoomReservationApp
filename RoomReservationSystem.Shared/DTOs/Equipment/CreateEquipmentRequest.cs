using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Shared.DTOs.Equipment
{
    public class CreateEquipmentRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
