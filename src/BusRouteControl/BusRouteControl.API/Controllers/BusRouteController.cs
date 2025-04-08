using BusRouteControl.API.Dtos;
using BusRouteControl.Domain.Core;
using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusRouteController : ControllerBase
    {
        private readonly BusRouteControlDbContext _context;

        public BusRouteController(BusRouteControlDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<BusRouteDto>>> GetAll()
        {
            var routes = await _context.BusRoutes
                .Include(r => r.Schedules)
                .ToListAsync();

            var routeDtos = routes.Select(r => new BusRouteDto
            {
                Id = r.Id,
                Name = r.Name,
                Origin = r.Origin,
                Destination = r.Destination,
                Schedules = r.Schedules.Select(s => new ScheduleDto
                {
                    Id = s.Id,
                    DepartureTime = s.DepartureTime.ToString("HH:mm"),
                    ArrivalTime = s.ArrivalTime.ToString("HH:mm")
                }).ToList()
            });
            return Ok(routeDtos);
        }
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<BusRouteDto>> GetBusRoute(int id)
        {
            var route = await _context.BusRoutes.Include(r => r.Schedules).FirstOrDefaultAsync(r => r.Id == id);

            if (route == null)
                return NotFound();

            return Ok(MapToDto(route));
        }
        [HttpPost("Create")]
        public async Task<ActionResult<BusRouteDto>> Create([FromBody] BusRouteDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data.");

            var busRoute = new BusRoute
            {
                Name = dto.Name,
                Origin = dto.Origin,
                Destination = dto.Destination
            };

            foreach (var s in dto.Schedules)
            {
                busRoute.Schedules.Add(new Schedule
                {
                    DepartureTime = TimeOnly.Parse(s.DepartureTime),
                    ArrivalTime = TimeOnly.Parse(s.ArrivalTime),
                    Route = busRoute
                });
            }

            _context.BusRoutes.Add(busRoute);
            await _context.SaveChangesAsync();

            dto.Id = busRoute.Id;
            dto.Schedules = busRoute.Schedules.Select(s => new ScheduleDto
            {
                Id = s.Id,
                DepartureTime = s.DepartureTime.ToString("HH:mm"),
                ArrivalTime = s.ArrivalTime.ToString("HH:mm")
            }).ToList();

            return CreatedAtAction(nameof(GetAll), new { id = dto.Id }, dto);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateBusRoute(int id, [FromBody] BusRouteDto dto)
        {
            var route = await _context.BusRoutes.Include(r => r.Schedules).FirstOrDefaultAsync(r => r.Id == id);
            if (route == null)
                return NotFound();

            route.Name = dto.Name;
            route.Origin = dto.Origin;
            route.Destination = dto.Destination;

            _context.Schedules.RemoveRange(route.Schedules);

            foreach (var s in dto.Schedules)
            {
                route.Schedules.Add(new Schedule
                {
                    DepartureTime = TimeOnly.Parse(s.DepartureTime),
                    ArrivalTime = TimeOnly.Parse(s.ArrivalTime),
                    Route = route
                });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBusRoute(int id)
        {
            var route = await _context.BusRoutes.Include(r => r.Schedules).FirstOrDefaultAsync(r => r.Id == id);
            if (route == null)
                return NotFound();

            _context.Schedules.RemoveRange(route.Schedules);
            _context.BusRoutes.Remove(route);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private BusRouteDto MapToDto(BusRoute route)
        {
            return new BusRouteDto
            {
                Id = route.Id,
                Name = route.Name,
                Origin = route.Origin,
                Destination = route.Destination,
                Schedules = route.Schedules.Select(s => new ScheduleDto
                {
                    Id = s.Id,
                    DepartureTime = s.DepartureTime.ToString("HH:mm"),
                    ArrivalTime = s.ArrivalTime.ToString("HH:mm")
                }).ToList()
            };
        }
    }
}
