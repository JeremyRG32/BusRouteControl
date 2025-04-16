using BusRouteControl.Application.Contracts;
using BusRouteControl.Domain.Dtos;
using BusRouteControl.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _scheduleService.GetAllSchedulesAsync();
            return Ok(result);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
                return NotFound(new { message = "Schedule not found." });

            return Ok(schedule);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ScheduleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("The model is invalid.");

            var model = new ScheduleModel
            {
                Id = dto.Id,
                RouteId = dto.RouteId,
                DepartureTime = dto.DepartureTime,
                ArrivalTime = dto.ArrivalTime
            };

            var created = await _scheduleService.CreateScheduleAsync(model);
            return Ok(new
            {
                message = "Schedule created successfully.",
                data = created
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ScheduleDto dto)
        {
            var model = new ScheduleModel
            {
                RouteId = dto.RouteId,
                DepartureTime = dto.DepartureTime,
                ArrivalTime = dto.ArrivalTime
            };
            var success = await _scheduleService.UpdateScheduleAsync(id, model);
            if (!success)
                return NotFound(new { message = "Schedule not found." });

            return Ok(new { message = "Schedule updated successfully." });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _scheduleService.DeleteScheduleAsync(id);
            if (!success)
                return NotFound(new { message = "Schedule not found." });

            return Ok(new { message = "Schedule deleted successfully." });
        }
    }
}
