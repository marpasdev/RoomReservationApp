using RoomReservationSystem.Shared.DTOs.Reservations;
using RoomReservationSystem.Web.Repositories;
using RoomReservationSystem.Shared.Models;
using RoomReservationSystem.Shared.Enums;

namespace RoomReservationSystem.Web.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
        }

        public async Task<int> CreateAsync(CreateReservationRequest request)
        {
            return await reservationRepository.CreateAsync(new Reservation()
            {
                RoomId = request.RoomId,
                Start = request.Start,
                End = request.End,
                Purpose = request.Purpose,
                PersonCount = request.PersonCount,
                BookerId = request.BookerId,
                Status = ReservationStatus.Active,
                CreatedAt = DateTime.UtcNow
            });
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReservationDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ReservationDto?> GetByUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReservationDto>> GetCurrentAsync(int roomId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateReservationRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateReservationAsync(UpdateReservationRequest request)
        {
            await reservationRepository.UpdateAsync(new Reservation()
            {
                Start = request.Start,
                End = request.End,
                Purpose = request.Purpose,
                PersonCount = request.PersonCount,
                Status = ReservationStatus.Active,
                CreatedAt = DateTime.UtcNow
                
            });
        }
    }
}
