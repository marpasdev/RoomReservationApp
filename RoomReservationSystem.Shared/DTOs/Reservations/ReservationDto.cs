using RoomReservationSystem.Shared.DTOs.Rooms;
using RoomReservationSystem.Shared.DTOs.Users;
using RoomReservationSystem.Shared.Enums;

namespace RoomReservationSystem.Shared.DTOs.Reservations
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public RoomDto Room { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public required string Purpose { get; set; }
        public int PersonCount { get; set; }
        public ReservationStatus Status { get; set; }
        public UserDto Booker { get; set; }
    }
}
