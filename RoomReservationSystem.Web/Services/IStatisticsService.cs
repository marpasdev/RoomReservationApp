using RoomReservationSystem.Shared.DTOs.Statistics;

namespace RoomReservationSystem.Web.Services
{
    public interface IStatisticsService
    {
        Task<IEnumerable<RoomStatisticsDto>> GetAllStatisticsAsync(DateTime from, DateTime to);
        Task<RoomStatisticsDto?> GetStatisticsAsync(DateTime from, DateTime to, int roomId);
    }
}
