using RoomReservationSystem.Shared.Models;

namespace RoomReservationSystem.Web.Repositories
{
    public interface IEquipmentRepository
    {
        public Task<int> CreateAsync(Equipment equipment);

        public Task DeleteAsync(int id);

        public Task UpdateAsync(Equipment equipment);

        public Task<IEnumerable<Equipment>> GetAllAsync();

        public Task<Equipment?> GetByIdAsync(int id);
    }
}
