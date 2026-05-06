using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationSystem.Shared.DTOs.Users;
using RoomReservationSystem.Web.Services;
using System.Security.Claims;

namespace RoomReservationSystem.Web.Controllers
{
    [Route("profile")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService userService;
        private readonly IReservationService reservationService;

        public ProfileController(IUserService userService, IReservationService reservationService)
        {
            this.userService = userService;
            this.reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            UserDto? user = await userService.GetByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpGet("reservations")]
        public async Task<IActionResult> GetReservations()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var reservations = await reservationService.GetByUserAsync(userId);
            return Json(reservations);
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            UserDto? user = await userService.GetByIdAsync(userId);
            if (user is null) { return NotFound(); }
            string email = user.Email;
            string userName = user.UserName;

            return View(new UpdateUserRequest()
            {
                Id = userId,
                Email = email,
                UserName = userName
            });
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(UpdateUserRequest request)
        {
            request.Id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            try
            {
                await userService.UpdateAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
        }

        [HttpGet("changepassword")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(UpdatePasswordRequest request)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            try
            {
                await userService.ChangePasswordAsync(request, userId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
        }

        [HttpGet("data")]
        public async Task<IActionResult> GetData()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            UserDto? user = await userService.GetByIdAsync(userId);
            return Json(user);
        }
    }
}
