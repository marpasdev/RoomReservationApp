using RoomReservationSystem.Shared.DTOs.Equipment;
using RoomReservationSystem.Shared.Models;

namespace RoomReservationSystem.Web.Services
{
    public interface IEquipmentService
    {
        Task<IEnumerable<Equipment>> GetAllAsync();
        Task<int> CreateAsync(CreateEquipmentRequest request);
        Task UpdateAsync(UpdateEquipmentRequest request);
        Task DeleteAsync(int id);
    }
}
