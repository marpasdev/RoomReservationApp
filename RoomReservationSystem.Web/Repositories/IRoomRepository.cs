using RoomReservationSystem.Shared.Models;

namespace RoomReservationSystem.Web.Repositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room?> GetByIdAsync(int id);
        Task<IEnumerable<Room>> GetAvailableAsync(DateTime from, DateTime to, int minCapacity);
        Task<int> CreateAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(int id);
    }
}
