using BusRouteControl.Infrastructure.Models;
using BusRouteControl.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleRepository _scheduleRepository;

        public ScheduleController(ScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _scheduleRepository.GetAll();
            return Ok(result);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _scheduleRepository.GetEntityById(id);
            if (schedule == null)
                return NotFound(new { message = "Schedule not found." });

            return Ok(schedule);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ScheduleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("The model is invalid.");

            var created = await _scheduleRepository.Create(model);
            return Ok(new
            {
                message = "Schedule created successfully.",
                data = created
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ScheduleModel model)
        {
            var success = await _scheduleRepository.Update(id, model);
            if (!success)
                return NotFound(new { message = "Schedule not found." });

            return Ok(new { message = "Schedule updated successfully." });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _scheduleRepository.DeleteEntity(id);
            if (!success)
                return NotFound(new { message = "Schedule not found." });

            return Ok(new { message = "Schedule deleted successfully." });
        }
    }
}
