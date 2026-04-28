using RoomReservationSystem.Shared.Models;

namespace RoomReservationSystem.Web.Repositories
{
    public interface IRoomEquipmentRepository
    {
        Task CreateRoomEquipmentAsync(RoomEquipment roomEquipment);
        Task DeleteRoomEquipmentAsync(int roomId, int equipmentId);
        Task UpdateRoomEquipmentAsync(RoomEquipment roomEquipment);
        Task<IEnumerable<RoomEquipment>> GetByRoomAsync(int roomId);
    }
}
