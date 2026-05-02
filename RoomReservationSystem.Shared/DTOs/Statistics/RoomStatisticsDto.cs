namespace RoomReservationSystem.Shared.DTOs.Statistics
{
    public class RoomStatisticsDto
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int TotalReservations { get; set; }
        public double OccupancyHours { get; set; }
        public double OccupancyPercentage { get; set; }
    }
}
