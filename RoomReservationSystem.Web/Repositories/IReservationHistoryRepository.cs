using RoomReservationSystem.Shared.Models;

namespace RoomReservationSystem.Web.Repositories
{
    public interface IReservationHistoryRepository
    {
        Task<IEnumerable<ReservationHistory>> GetAllAsync();
        Task<IEnumerable<ReservationHistory>> GetByUserAsync(int userId);
        Task<int> CreateAsync(ReservationHistory history);
        Task DeleteAsync(int id);
    }
}
