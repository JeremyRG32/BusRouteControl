using BusRouteControl.API.Dtos;
using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly BusRouteControlDbContext _context;

        public ScheduleController(BusRouteControlDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedules()
        {
            var schedules = await _context.Schedules
                .Select(s => new ScheduleDto
                {
                    Id = s.Id,
                    RouteId = s.RouteId,
                    DepartureTime = s.DepartureTime.ToString("HH:mm"),
                    ArrivalTime = s.ArrivalTime.ToString("HH:mm")
                })
                .ToListAsync();

            return Ok(schedules);
        }
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<ScheduleDto>> GetSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule == null)
                return NotFound();

            var dto = new ScheduleDto
            {
                Id = schedule.Id,
                RouteId = schedule.RouteId,
                DepartureTime = schedule.DepartureTime.ToString("HH:mm"),
                ArrivalTime = schedule.ArrivalTime.ToString("HH:mm")
            };

            return Ok(dto);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<ScheduleDto>> CreateSchedule(ScheduleDto scheduleDto)
        {
            if (!TimeOnly.TryParse(scheduleDto.DepartureTime, out var departure) ||
                !TimeOnly.TryParse(scheduleDto.ArrivalTime, out var arrival))
            {
                return BadRequest("Invalid date format. Use 'HH:mm'.");
            }

            var schedule = new Schedule
            {
                RouteId = scheduleDto.RouteId,
                DepartureTime = departure,
                ArrivalTime = arrival
            };

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            scheduleDto.Id = schedule.Id;
            return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, scheduleDto);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, ScheduleDto scheduleDto)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
                return NotFound();

            if (!TimeOnly.TryParse(scheduleDto.DepartureTime, out var departure) ||
                !TimeOnly.TryParse(scheduleDto.ArrivalTime, out var arrival))
            {
                return BadRequest("Invalid date format. Use 'HH:mm'.");
            }

            schedule.RouteId = scheduleDto.RouteId;
            schedule.DepartureTime = departure;
            schedule.ArrivalTime = arrival;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
                return NotFound();

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
