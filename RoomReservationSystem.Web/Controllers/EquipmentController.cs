using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationSystem.Shared.DTOs.Equipment;
using RoomReservationSystem.Shared.Models;
using RoomReservationSystem.Web.Services;

namespace RoomReservationSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            this.equipmentService = equipmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetAllAsync()
        {
            var equipment = await equipmentService.GetAllAsync();
            return Ok(equipment);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateEquipmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int id = await equipmentService.CreateAsync(request);
            return Ok(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateEquipmentRequest request)
        {
            if (id != request.Id) return BadRequest("Id mismatch.");
            await equipmentService.UpdateAsync(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await equipmentService.DeleteAsync(id);
            return Ok();
        }
    }
}
