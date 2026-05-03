using Microsoft.AspNetCore.Mvc;
using RoomReservationSystem.Web.Services;
using RoomReservationSystem.Shared.DTOs.Auth;

namespace RoomReservationSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService authService;

        public AccountController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid) { return View(request); }
            try
            {
                var response = await authService.LoginAsync(request);
                if (response is null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(request);
                }
                Response.Cookies.Append("jwt", response.Token);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid) { return View(request); }
            try
            {
                await authService.RegisterAsync(request);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Index", "Home");
        }
    }
}
