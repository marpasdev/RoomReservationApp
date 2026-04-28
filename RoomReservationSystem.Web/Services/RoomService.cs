using RoomReservationSystem.Shared.DTOs.Rooms;
using RoomReservationSystem.Shared.DTOs.Equipment;
using RoomReservationSystem.Shared.Models;
using RoomReservationSystem.Web.Repositories;
using System.Linq;

namespace RoomReservationSystem.Web.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository roomRepository;
        private readonly IEquipmentRepository roomEquipmentRepository;

        public RoomService(IRoomRepository roomRepository, IEquipmentRepository equipmentRepository)
        {
            this.roomRepository = roomRepository;
            this.equipmentRepository = equipmentRepository;
        }

        public async Task<int> CreateAsync(CreateRoomRequest request)
        {
            return await roomRepository.CreateAsync(new Room()
            {
                Name = request.Name, Capacity = request.Capacity,
                MaxReservationMinutes = request.MaxReservationMinutes
            });
        }

        public async Task DeleteAsync(int id)
        {
            await roomRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
        {
            var rooms = await roomRepository.GetAllAsync();
            return rooms.Select(room => new RoomDto()
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                MaxReservationMinutes = room.MaxReservationMinutes
            });
        }

        public Task<IEnumerable<RoomDto>> GetAvailableAsync(DateTime start, DateTime end, int minCapacity)
        {
            throw new NotImplementedException();
        }

        public async Task<RoomDto?> GetByIdAsync(int id)
        {
            Room? room = await roomRepository.GetByIdAsync(id);
            if (room is null)
            {
                return null;
            }

            var equipment = equipmentRepository.GetByIdAsync


            return new RoomDto() { Id = room.Id, Capacity = room.Capacity,
            Name = room.Name, MaxReservationMinutes = room.MaxReservationMinutes,
            CurrentReservation = null, IsAvailable = null, Equipment = null };
        }

        public async Task UpdateAsync(UpdateRoomRequest request)
        {
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
