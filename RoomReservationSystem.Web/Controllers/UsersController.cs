using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationSystem.Shared.DTOs.Users;
using RoomReservationSystem.Web.Services;
using System.Security.Claims;

namespace RoomReservationSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }


        //Task<IEnumerable<UserDto>> GetAllAsync();
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await userService.GetAllAsync();
            return Ok(users);
        }

        //Task<UserDto?> GetByIdAsync(int id);
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var user = await userService.GetByIdAsync(id);
            if (user is null) { return NotFound(); }
            return Ok(user);
        }

        //Task UpdateAsync(UpdateUserRequest request);
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateUserRequest request)
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
                await userService.UpdateAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Task DeleteAsync(int id);
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await userService.DeleteAsync(id);
            return Ok();
        }

        //Task ChangePasswordAsync(UpdatePasswordRequest request, int userId);
        [HttpPut("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] UpdatePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                await userService.ChangePasswordAsync(request, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
