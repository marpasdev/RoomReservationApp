using RoomReservationSystem.Shared.DTOs.Rooms;
using RoomReservationSystem.Shared.DTOs.Equipment;
using RoomReservationSystem.Shared.Models;
using RoomReservationSystem.Web.Repositories;
using RoomReservationSystem.Shared.DTOs.Reservations;

namespace RoomReservationSystem.Web.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository roomRepository;
        private readonly IRoomEquipmentRepository roomEquipmentRepository;
        private readonly IReservationRepository reservationRepository;

        public RoomService(IRoomRepository roomRepository, IRoomEquipmentRepository roomEquipmentRepository, IReservationRepository reservationRepository)
        {
            this.roomRepository = roomRepository;
            this.roomEquipmentRepository = roomEquipmentRepository;
            this.reservationRepository = reservationRepository;
        }

        public async Task<int> CreateAsync(CreateRoomRequest request)
        {
            int roomId = await roomRepository.CreateAsync(new Room()
            {
                Name = request.Name, Capacity = request.Capacity,
                MaxReservationMinutes = request.MaxReservationMinutes
            });

            foreach (RoomEquipmentRequest e in request.Equipment) {
                await roomEquipmentRepository.CreateAsync(new RoomEquipment()
                {
                    EquipmentId = e.EquipmentId,
                    RoomId = roomId,
                    Quantity = e.Quantity
                });
            }

            return roomId;
        }

        public async Task DeleteAsync(int id)
        {
            await roomRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
        {
            var rooms = await roomRepository.GetAllAsync();
            return await Task.WhenAll(rooms.Select(async (r) =>
                {
                    IEnumerable<EquipmentDto> equipment = await roomEquipmentRepository.GetByRoomAsync(r.Id);
                    Reservation? reservation = await reservationRepository.GetCurrentAsync(r.Id);
                    ReservationDto? reservationDto = null;
                    if (reservation is not null)
                    {
                        reservationDto = new ReservationDto()
                        {
                            Id = reservation.Id,
                            Start = reservation.Start,
                            End = reservation.End,
                            BookerId = reservation.BookerId,
                            RoomId = reservation.RoomId,
                            PersonCount = reservation.PersonCount,
                            Purpose = reservation.Purpose,
                            Status = reservation.Status
                        };
                    }
                    return new RoomDto()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Capacity = r.Capacity,
                        MaxReservationMinutes = r.MaxReservationMinutes,
                        IsAvailable = (reservationDto is null),
                        Equipment = equipment,
                        CurrentReservation = reservationDto
                    };          
                }
            ));
        }

        public async Task<IEnumerable<RoomDto>> GetAvailableAsync(DateTime start, DateTime end, int minCapacity)
        {
            var rooms = await roomRepository.GetAvailableAsync(start, end, minCapacity);

            return await Task.WhenAll(rooms.Select(async (r) => 
            {
                IEnumerable<EquipmentDto> equipment = await roomEquipmentRepository.GetByRoomAsync(r.Id);
                return new RoomDto()
                {
                    Name = r.Name,
                    Capacity = r.Capacity,
                    MaxReservationMinutes = r.MaxReservationMinutes,
                    Id = r.Id,
                    IsAvailable = true,
                    Equipment = equipment
                };
            }));
        }

        public async Task<RoomDto?> GetByIdAsync(int id)
        {
            Room? room = await roomRepository.GetByIdAsync(id);
            if (room is null)
            {
                return null;
            }

            var equipment = await roomEquipmentRepository.GetByRoomAsync(id);

            Reservation? reservation = await reservationRepository.GetCurrentAsync(room.Id);

            ReservationDto? reservationDto = null;

            if (reservation is not null)
            {
                reservationDto = new ReservationDto()
                {
                    Id = reservation.Id,
                    Start = reservation.Start,
                    End = reservation.End,
                    BookerId = reservation.BookerId,
                    RoomId = reservation.RoomId,
                    PersonCount = reservation.PersonCount,
                    Purpose = reservation.Purpose,
                    Status = reservation.Status
                };
            }

            return new RoomDto() { Id = room.Id, Capacity = room.Capacity,
            Name = room.Name, MaxReservationMinutes = room.MaxReservationMinutes,
            CurrentReservation = reservationDto, IsAvailable = (reservationDto is null), Equipment = equipment,
            };
        }

        public async Task UpdateAsync(UpdateRoomRequest request)
        {
            await roomEquipmentRepository.DeleteByRoomAsync(request.Id);

            foreach (RoomEquipmentRequest e in request.Equipment)
            {
                await roomEquipmentRepository.CreateAsync(new RoomEquipment()
                {
                    EquipmentId = e.EquipmentId,
                    RoomId = request.Id,
                    Quantity = e.Quantity
                });
            }
            await roomRepository.UpdateAsync(new Room()
            {
                Id = request.Id,
                Name = request.Name,
                Capacity = request.Capacity,
                MaxReservationMinutes = request.MaxReservationMinutes
            });
        }
    }
}
