using System.ComponentModel.DataAnnotations;
using System.Formats.Asn1;
using System.Net.Http;
using System.Net.Http.Json;
using RoomReservationSystem.Shared.DTOs.Equipment;
using RoomReservationSystem.Shared.DTOs.Reservations;
using RoomReservationSystem.Shared.DTOs.Rooms;
using RoomReservationSystem.Shared.DTOs.Statistics;
using RoomReservationSystem.Shared.DTOs.Users;
using RoomReservationSystem.Shared.Models;

namespace RoomReservationSystem.Admin
{
    public class ApiClient
    {
        private readonly HttpClient http;

        public ApiClient(string url)
        {
            http = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
        }

        public void SetToken(string token)
        {
            http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        #region Rooms
        public async Task<List<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await http.GetFromJsonAsync<IEnumerable<RoomDto>>("api/rooms");
            if (rooms is not null)
            {
                return rooms.ToList();
            } else
            {
                return new List<RoomDto>();
            }
        }

        public async Task<RoomDto?> GetRoomByIdAsync(int id)
        {
            RoomDto? room = await http.GetFromJsonAsync<RoomDto?>($"api/rooms/{id}");
            return room;
        }

        public async Task<int> CreateRoomAsync(CreateRoomRequest request)
        {
            var response = await http.PostAsJsonAsync("api/rooms", request);
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task UpdateRoomAsync(UpdateRoomRequest request)
        {
            await http.PutAsJsonAsync($"api/rooms/{request.Id}", request);
        }

        public async Task DeleteRoomAsync(int id)
        {
            await http.DeleteAsync($"api/rooms/{id}");
        }
        #endregion

        #region Equipment
        public async Task<List<Equipment>> GetAllEquipmentAsync()
        {
            var equipment = await http.GetFromJsonAsync<IEnumerable<Equipment>>(
                $"api/equipment");
            if (equipment is not null)
            {
                return equipment.ToList();
            }
            else
            {
                return new List<Equipment>();
            }
        }

        public async Task<int> CreateEquipmentAsync(CreateEquipmentRequest request)
        {
            var response = await http.PostAsJsonAsync("api/equipment", request);
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task UpdateEquipmentAsync(UpdateEquipmentRequest request)
        {
            await http.PutAsJsonAsync($"api/equipment/{request.Id}", request);
        }

        public async Task DeleteEquipmentAsync(int id)
        {
            await http.DeleteAsync($"api/equipment/{id}");
        }
        #endregion

        #region User
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await http.GetFromJsonAsync<IEnumerable<UserDto>>(
                $"api/users");
            if (users is not null)
            {
                return users.ToList();
            }
            else
            {
                return new List<UserDto>();
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            UserDto? user = await http.GetFromJsonAsync<UserDto?>(
                $"api/users/{id}");
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            await http.DeleteAsync($"api/users/{id}");
        }
        #endregion

        #region Statistics
        public async Task<List<RoomStatisticsDto>> GetAllStatisticsAsync(DateTime from, DateTime to)
        {
            var stats = await http.GetFromJsonAsync<IEnumerable<RoomStatisticsDto>>
                ($"api/statistics?from={from:yyyy-MM-ddTHH:mm:ss}&{to:yyyy-MM-ddTHH:mm:ss}");
            if (stats is not null)
            {
                return stats.ToList();
            }
            else
            {
                return new List<RoomStatisticsDto>();
            }
        }

        public async Task<RoomStatisticsDto?> GetStatisticsAsync(DateTime from, DateTime to, int roomId)
        {
            RoomStatisticsDto? stats = await http.GetFromJsonAsync<RoomStatisticsDto?>(
                $"api/statistics/{roomId}?from={from:yyyy-MM-ddTHH:mm:ss}&to={to:yyyy-MM-ddTHH:mm:ss}");
            return stats;
        }

        #endregion

        #region Reservations
        public async Task<List<ReservationDto>> GetAllReservations()
        {
            var reservations = await http.GetFromJsonAsync<IEnumerable<ReservationDto>>(
                $"api/reservations");
            if (reservations is not null)
            {
                return reservations.ToList();
            }
            else
            {
                return new List<ReservationDto>();
            }
        }

        public async Task<List<ReservationDto>> GetByUserAsync(int userId)
        {
            var reservations = await http.GetFromJsonAsync<IEnumerable<ReservationDto>>(
                $"api/reservations/user/{userId}");
            if (reservations is not null)
            {
                return reservations.ToList();
            }
            else
            {
                return new List<ReservationDto>();
            }
        }
        public async Task DeleteReservationAsync(int id)
        {
            await http.DeleteAsync($"api/reservations/{id}");
        }

        #endregion

    }
}
