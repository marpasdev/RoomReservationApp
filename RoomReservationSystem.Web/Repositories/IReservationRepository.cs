using RoomReservationSystem.Shared.Models;

namespace RoomReservationSystem.Web.Repositories
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<IEnumerable<Reservation>> GetByUserAsync(int userId);
        Task<Reservation?> GetByIdAsync(int id);
        Task<Reservation?> GetCurrentAsync(int roomId);
        Task<int> CreateAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(int id);
        Task<bool> HasCollisionAsync(int roomId, DateTime start, DateTime end, int? reservationId = null);
        Task<IEnumerable<Reservation>> GetByRoomAndPeriodAsync(int roomId, DateTime from, DateTime to);
    }
}
