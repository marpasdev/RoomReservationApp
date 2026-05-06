using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.Shared.DTOs.Equipment
{
    public class EquipmentDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}
