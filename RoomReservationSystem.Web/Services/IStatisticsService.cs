using RoomReservationSystem.Shared.DTOs.Statistics;

namespace RoomReservationSystem.Web.Services
{
    public interface IStatisticsService
    {
        Task<IEnumerable<RoomStatisticsDto>> GetAllAsync(DateTime from, DateTime to);
        Task<RoomStatisticsDto?> GetAsync(DateTime from, DateTime to, int roomId);
    }
}
