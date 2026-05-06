using Microsoft.AspNetCore.Mvc;
using RoomReservationSystem.Shared.DTOs.Statistics;
using RoomReservationSystem.Web.Services;

namespace RoomReservationSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomStatisticsDto>>> GetAllAsync([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var statistics = await statisticsService.GetAllAsync(from, to);
            return Ok(statistics);
        }

        [HttpGet("{roomId}")]
        public async Task<ActionResult<RoomStatisticsDto?>> GetAsync([FromQuery] DateTime from, [FromQuery] DateTime to, [FromRoute] int roomId)
        {
            var statistics = await statisticsService.GetAsync(from, to, roomId);
            return Ok(statistics);
        }
    }
}
