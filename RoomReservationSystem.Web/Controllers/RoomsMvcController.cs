using Microsoft.AspNetCore.Mvc;
using RoomReservationSystem.Shared.DTOs.Rooms;
using RoomReservationSystem.Web.Services;

namespace RoomReservationSystem.Web.Controllers
{
    [Route("rooms")]
    public class RoomsMvcController : Controller
    {
        private readonly IRoomService roomService;

        public RoomsMvcController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? from, DateTime? to, int minCapacity = 1) 
        {
            IEnumerable<RoomDto> rooms;

            if (from.HasValue && to.HasValue)
            {
                rooms = await roomService.GetAvailableAsync(from.Value, to.Value, minCapacity);
            }
            else
            {
                rooms = await roomService.GetAllAsync();
            }

            ViewBag.From = from;
            ViewBag.To = to;
            ViewBag.MinCapacity = minCapacity;

            return View(rooms);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var room = await roomService.GetByIdAsync(id);
            if (room is null) { return NotFound(); }
            return View(room);
        }
    }
}
