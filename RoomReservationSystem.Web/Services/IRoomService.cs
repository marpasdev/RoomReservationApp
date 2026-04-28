using RoomReservationSystem.Shared.DTOs.Rooms;

namespace RoomReservationSystem.Web.Services
{
    public interface IRoomService
    {
        Task<int> CreateAsync(CreateRoomRequest request);
        Task UpdateAsync(UpdateRoomRequest request);
        Task DeleteAsync(int id);
        Task<IEnumerable<RoomDto>> GetAllAsync();
        Task<RoomDto?> GetByIdAsync(int id);
        Task<IEnumerable<RoomDto>> GetAvailableAsync(DateTime start, DateTime end, int minCapacity);
    }
}
