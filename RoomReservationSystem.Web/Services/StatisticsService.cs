using RoomReservationSystem.Shared.Models;
using RoomReservationSystem.Shared.DTOs.Statistics;
using RoomReservationSystem.Web.Repositories;

namespace RoomReservationSystem.Web.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IRoomRepository roomRepository;

        public StatisticsService(IReservationRepository reservationRepository, IRoomRepository roomRepository)
        {
            this.reservationRepository = reservationRepository;
            this.roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomStatisticsDto>> GetAllStatisticsAsync(DateTime from, DateTime to)
        {
            IEnumerable<Room> rooms = await roomRepository.GetAllAsync();

            IEnumerable<RoomStatisticsDto?> stats = await Task.WhenAll(rooms.Select(async r =>
            {
                return await GetStatisticsAsync(from, to, r.Id);
            }));

            return stats!;
        }

        public async Task<RoomStatisticsDto?> GetStatisticsAsync(DateTime from, DateTime to, int roomId)
        {
            IEnumerable<Reservation> reservations = await 
                reservationRepository.GetByRoomAndPeriodAsync(roomId, from, to);

            Room? room = await roomRepository.GetByIdAsync(roomId);

            if (room is null)
            {
                throw new Exception("Room not found");
            }

            double occupancyHours = reservations.Select(r => (r.End - r.Start).TotalHours).Sum();

            return new RoomStatisticsDto()
            {
                RoomId = roomId,
                RoomName = room.Name,
                TotalReservations = reservations.Count(),
                OccupancyHours = occupancyHours,
                OccupancyPercentage = (occupancyHours / (to - from).TotalHours) * 100
            };
        }
    }
}
