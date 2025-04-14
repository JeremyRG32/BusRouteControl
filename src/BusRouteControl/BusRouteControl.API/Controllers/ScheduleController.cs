using BusRouteControl.Domain.Dtos;
using BusRouteControl.Infrastructure.Contracts;
using BusRouteControl.Infrastructure.Interfaces;
using BusRouteControl.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleController(IScheduleRepository scheduleRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _scheduleRepository = scheduleRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.Schedules.GetAll();
            return Ok(result);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _unitOfWork.Schedules.GetEntityById(id);
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

            var created = await _unitOfWork.Schedules.Create(model);
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
            var success = await _unitOfWork.Schedules.Update(id, model);
            if (!success)
                return NotFound(new { message = "Schedule not found." });

            return Ok(new { message = "Schedule updated successfully." });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _unitOfWork.Schedules.DeleteEntity(id);
            if (!success)
                return NotFound(new { message = "Schedule not found." });

            return Ok(new { message = "Schedule deleted successfully." });
        }
    }
}
