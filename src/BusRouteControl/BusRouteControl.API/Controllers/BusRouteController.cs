using BusRouteControl.Application.Contracts;
using BusRouteControl.Domain.Dtos;
using BusRouteControl.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusRouteController : ControllerBase
    {
        private readonly IBusRouteService _busrouteService;

        public BusRouteController(IBusRouteService busrouteService)
        {
            _busrouteService = busrouteService;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _busrouteService.GetAllAsync());
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetBusRoute(int id)
        {
            return Ok(await _busrouteService.GetByIdAsync(id));
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BusRouteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The Model is Invalid.");
            }
            var model = new BusRouteModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Origin = dto.Origin,
                Destination = dto.Destination,
                DefaultPrice = dto.DefaultPrice,
                Schedules = dto.Schedules.Select(s => new ScheduleModel
                {
                    DepartureTime = s.DepartureTime.ToString(),
                    ArrivalTime = s.ArrivalTime.ToString()
                }).ToList()
            };
            model = await _busrouteService.CreateAsync(model);
            return Ok(new
            {
                message = "Bus route created successfully.",
                data = model
            });
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateBusRoute(int id, [FromBody] BusRouteDto dto)
        {
            var model = new BusRouteModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Origin = dto.Origin,
                Destination = dto.Destination,
                DefaultPrice = dto.DefaultPrice,
                Schedules = dto.Schedules.Select(s => new ScheduleModel
                {
                    Id = s.Id,
                    DepartureTime = s.DepartureTime.ToString(),
                    ArrivalTime = s.ArrivalTime.ToString()
                }).ToList()
            };
            var success = await _busrouteService.UpdateAsync(id, model);
            if (!success)
                return NotFound();

            return Ok(new { message = "Bus route updated successfully." });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBusRoute(int id)
        {
            var success = await _busrouteService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return Ok(new { message = "Bus route deleted successfully." });
        }
    }
}
