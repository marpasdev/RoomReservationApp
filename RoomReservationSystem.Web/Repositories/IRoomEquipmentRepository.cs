using RoomReservationSystem.Shared.Models;
using RoomReservationSystem.Shared.DTOs.Equipment;

namespace RoomReservationSystem.Web.Repositories
{
    public interface IRoomEquipmentRepository
    {
        Task CreateAsync(RoomEquipment roomEquipment);
        Task DeleteAsync(int roomId, int equipmentId);
        Task DeleteByRoomAsync(int roomId);
        Task UpdateAsync(RoomEquipment roomEquipment);
        Task<IEnumerable<EquipmentDto>> GetByRoomAsync(int roomId);
    }
}
