using Microsoft.AspNetCore.Mvc;
using RoomReservationSystem.Shared.DTOs.Reservations;
using RoomReservationSystem.Web.Services;
using System.Security.Claims;

namespace RoomReservationSystem.Web.Controllers
{
    [Route("reservations")]
    public class ReservationsMvcController : Controller
    {
        private readonly IReservationService reservationService;

        public ReservationsMvcController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            IEnumerable<ReservationDto> reservations = await reservationService.GetByUserAsync(userId);

            return View(reservations);
        }

        [HttpGet("create/{roomId}")]
        public IActionResult Create(int roomId)
        {
            return View(new CreateReservationRequest()
            {
                RoomId = roomId
            });
        }

        [HttpPost("create/{roomId}")]
        public async Task<IActionResult> Create(int roomId, CreateReservationRequest request)
        {

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            request.BookerId = userId;
            request.RoomId = roomId;
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            try
            {
                await reservationService.CreateAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int id)
        {
            ReservationDto? reservation = await reservationService.GetByIdAsync(id);
            if (reservation is null) { return NotFound(); }
            return View(reservation);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(UpdateReservationRequest request)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            try
            {
                await reservationService.UpdateAsync(request, userId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
            return RedirectToAction("Index");
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await reservationService.CancelAsync(id, userId);
            return RedirectToAction("Index");
        }
    }
}
