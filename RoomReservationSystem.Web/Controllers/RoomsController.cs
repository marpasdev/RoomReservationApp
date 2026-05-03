using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationSystem.Shared.DTOs.Rooms;
using RoomReservationSystem.Web.Services;

namespace RoomReservationSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService roomService;

        public RoomsController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var rooms = await roomService.GetAllAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var room = await roomService.GetByIdAsync(id);
            if (room is null) { return NotFound(); }
            return Ok(room);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableAsync(
            [FromQuery] DateTime start, 
            [FromQuery] DateTime end,
            [FromQuery] int minCapacity)
        {
            var rooms = await roomService.GetAvailableAsync(start, end, minCapacity);
            return Ok(rooms);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRoomRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var id = await roomService.CreateAsync(request);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateRoomRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("Id mismatch.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await roomService.UpdateAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await roomService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
