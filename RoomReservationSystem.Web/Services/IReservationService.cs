using RoomReservationSystem.Shared.DTOs.Reservations;

namespace RoomReservationSystem.Web.Services
{
    public interface IReservationService
    {
        Task<int> CreateAsync(CreateReservationRequest request);
        Task UpdateAsync(UpdateReservationRequest request);
        Task DeleteAsync(int id);
        Task<IEnumerable<ReservationDto>> GetAllAsync();
        Task<ReservationDto?> GetByUserAsync(int userId);
        Task<IEnumerable<ReservationDto>> GetCurrentAsync(int roomId);

    }
}
