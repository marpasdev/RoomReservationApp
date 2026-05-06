namespace RoomReservationSystem.Admin
{
    public class EquipmentSelectionItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
