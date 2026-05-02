using RoomReservationSystem.Shared.DTOs.Reservations;

namespace RoomReservationSystem.Web.Services
{
    public interface IReservationService
    {
        Task<int> CreateAsync(CreateReservationRequest request);
        Task UpdateAsync(UpdateReservationRequest request, int userId);
        Task DeleteAsync(int id);
        Task<ReservationDto?> GetByIdAsync(int id);
        Task<IEnumerable<ReservationDto>> GetAllAsync();
        Task<IEnumerable<ReservationDto>> GetByUserAsync(int userId);
        Task<ReservationDto?> GetCurrentAsync(int roomId);
        Task CancelAsync(int id, int userId);

    }
}
