using RoomReservationSystem.Shared.DTOs.Reservations;
using RoomReservationSystem.Web.Repositories;
using RoomReservationSystem.Shared.Models;
using RoomReservationSystem.Shared.Enums;

namespace RoomReservationSystem.Web.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IReservationHistoryRepository historyRepository;

        public ReservationService(IReservationRepository reservationRepository, IRoomRepository roomRepository,
            IReservationHistoryRepository historyRepository)
        {
            this.reservationRepository = reservationRepository;
            this.roomRepository = roomRepository;
            this.historyRepository = historyRepository;
        }

        public async Task CancelAsync(int id, int userId)
        {
            Reservation? res = await reservationRepository.GetByIdAsync(id);
            if (res is null)
            {
                throw new Exception("Invalid not found.");
            }

            if (res.Status == ReservationStatus.Cancelled)
            {
                throw new Exception("Reservation already cancelled");
            }

            if (res.Start <=  DateTime.Now)
            {
                throw new Exception("A reservation that has already started cannot be cancelled");
            }

            await reservationRepository.UpdateAsync(new Reservation()
            {
                Id = id,
                Status = ReservationStatus.Cancelled
            });

            await historyRepository.CreateAsync(new ReservationHistory()
            {
                ChangedAt = DateTime.UtcNow,
                ChangedByUserId = userId,
                OldStatus = ReservationStatus.Active,
                NewStatus = ReservationStatus.Cancelled,
                ReservationId = id
            });
        }

        public async Task<int> CreateAsync(CreateReservationRequest request)
        {
            if (request.Start <  DateTime.Now)
            {
                throw new Exception("Cannot create a reservation starting in the past.");
            }

            Room? room = await roomRepository.GetByIdAsync(request.RoomId);

            if (room is null) { throw new Exception("Room not found."); }

            if (request.PersonCount > room.Capacity)
            {
                throw new Exception("Person count exceeds capacity of given room.");
            }

            double duration = (request.End - request.Start).TotalMinutes;
            if (duration > room.MaxReservationMinutes)
            {
                throw new Exception("Maximal reservation duration exceeded.");
            }

            if (await reservationRepository.HasCollisionAsync(request.RoomId, request.Start, request.End, null))
            {
                throw new Exception("Reservation cannot be created due to collision with another reservation.");
            }

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

        public async Task DeleteAsync(int id)
        {
            await reservationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ReservationDto>> GetAllAsync()
        {
            IEnumerable<Reservation> reservations = await reservationRepository.GetAllAsync();
            return reservations.Select(r => new ReservationDto() {
                RoomId = r.RoomId,
                Start = r.Start,
                End = r.End,
                Purpose = r.Purpose,
                PersonCount = r.PersonCount,
                BookerId = r.BookerId,
                Status = r.Status
            });
        }

        public async Task<ReservationDto?> GetByIdAsync(int id)
        {
            Reservation? res = await reservationRepository.GetByIdAsync(id);

            if (res is null)
            {
                throw new Exception("Invalid reservation id");
            }

            return new ReservationDto()
            {
                Id = res.Id,
                BookerId = res.BookerId,
                Start = res.Start,
                End = res.End,
                RoomId = res.RoomId,
                PersonCount = res.PersonCount,
                Purpose = res.Purpose,
                Status = res.Status
            };
        }

        public async Task<IEnumerable<ReservationDto>> GetByUserAsync(int userId)
        {
            IEnumerable<Reservation> reservations = await reservationRepository.GetByUserAsync(userId);
            return reservations.Select(r => new ReservationDto()
            {
                RoomId = r.RoomId,
                Start = r.Start,
                End = r.End,
                Purpose = r.Purpose,
                PersonCount = r.PersonCount,
                BookerId = r.BookerId,
                Status = r.Status
            });
        }

        public async Task<ReservationDto?> GetCurrentAsync(int roomId)
        {
            Reservation? reservation = await reservationRepository.GetCurrentAsync(roomId);
            if (reservation is null) { return null; }
            return new ReservationDto()
            {
                Id = reservation.Id,
                RoomId = reservation.RoomId,
                Start = reservation.Start,
                End = reservation.End,
                BookerId = reservation.BookerId,
                PersonCount = reservation.PersonCount,
                Purpose = reservation.Purpose,
                Status = reservation.Status
            };
        }

        public async Task UpdateAsync(UpdateReservationRequest request, int userId)
        {
            Reservation? res = await reservationRepository.GetByIdAsync(request.Id);

            if (res is null) 
            {
                throw new Exception("Invalid reservation.");
            }

            if (res.Status == ReservationStatus.Cancelled)
            {
                throw new Exception("A cancelled reservation cannot be updated.");
            }
            if (res.Start <= DateTime.UtcNow)
            {
                throw new Exception("Cannot update a reservation that has already started.");
            }

            Room? room = await roomRepository.GetByIdAsync(res.RoomId);
            if (room is null)
            {
                throw new Exception("Invalid room.");
            }

            if (request.Start < DateTime.UtcNow)
            {
                throw new Exception("Cannot update a reservation to start in the past");
            }

            if (request.PersonCount > room.Capacity)
            {
                throw new Exception("Person count exceeds capacity of given room.");
            }

            double duration = (request.End - request.Start).TotalMinutes;
            if (duration > room.MaxReservationMinutes)
            {
                throw new Exception("Maximal reservation duration exceeded.");
            }

            if (await reservationRepository.HasCollisionAsync(room.Id, request.Start, request.End, request.Id))
            {
                throw new Exception("Reservation cannot be created due to collision with another reservation.");
            }


            await historyRepository.CreateAsync(new ReservationHistory()
            {
                ChangedAt = DateTime.UtcNow,
                OldStatus = res.Status,
                NewStatus = res.Status,
                ReservationId = res.Id,
                ChangedByUserId = userId
            });

            await reservationRepository.UpdateAsync(new Reservation()
            {
                Id = request.Id,
                Start = request.Start,
                End = request.End,
                Purpose = request.Purpose,
                PersonCount = request.PersonCount,
                Status = ReservationStatus.Active
            });

        }
    }
}
