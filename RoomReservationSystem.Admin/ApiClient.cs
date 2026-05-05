using System.Formats.Asn1;
using System.Net.Http;
using System.Net.Http.Json;
using RoomReservationSystem.Shared.DTOs.Rooms;

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

        public async Task<List<RoomDto>> GetAllRoomsAsync()
        {
            try
            {
                var rooms = await http.GetFromJsonAsync<IEnumerable<RoomDto>>("rooms");
                if (rooms is not null)
                {
                    return rooms.ToList();
                } else
                {
                    return new List<RoomDto>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading data from API", ex);
            }
        }

        public async Task<RoomDto?> GetRoomByIdAsync(int id)
        {
            try
            {
                RoomDto? room = await http.GetFromJsonAsync<RoomDto?>("rooms/{id}");
                return room;
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading data from API", ex);
            }
        }

        public async Task<int> CreateRoomAsync()
        {

        }

        public async Task DeleteRoomAsync()
        {

        }

        public async Task DeleteUserAsync()
        {
            
        }



    }
}
