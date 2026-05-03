namespace RoomReservationSystem.Shared.DTOs.ApiDescription
{
    public class ApiEndpointDto
    {
        public string Controller { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string HttpMethod { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public List<ApiParameterDto> Parameters { get; set; } = new();
        public string Returns { get; set; } = string.Empty;
        public bool RequiresAuth { get; set; }
    }
}
