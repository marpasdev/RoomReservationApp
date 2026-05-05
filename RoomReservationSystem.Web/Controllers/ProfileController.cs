using Microsoft.AspNetCore.Mvc;
using RoomReservationSystem.Shared.DTOs.Users;
using RoomReservationSystem.Web.Services;
using System.Security.Claims;

namespace RoomReservationSystem.Web.Controllers
{
    [Route("profile")]
    public class ProfileController : Controller
    {
        private readonly IUserService userService;

        public ProfileController(IUserService userService)
        {
            this.userService = userService; 
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

        [HttpGet("update")]
        public async Task<IActionResult> UpdateProfile()
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

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateUserRequest request)
        {
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

        [HttpPost]
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
