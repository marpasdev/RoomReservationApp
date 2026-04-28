using RoomReservationSystem.Shared.Models;

namespace RoomReservationSystem.Web.Repositories
{
    public interface IEquipmentRepository
    {
        Task<int> CreateAsync(Equipment equipment);

        Task DeleteAsync(int id);

        Task UpdateAsync(Equipment equipment);

        Task<IEnumerable<Equipment>> GetAllAsync();

        Task<Equipment?> GetByIdAsync(int id);
    }
}
