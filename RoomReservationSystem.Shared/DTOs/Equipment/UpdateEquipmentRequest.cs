using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Shared.DTOs.Equipment
{
    public class UpdateEquipmentRequest
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
