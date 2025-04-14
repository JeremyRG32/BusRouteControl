using BusRouteControl.Domain.Dtos;
using BusRouteControl.Infrastructure.Contracts;
using BusRouteControl.Infrastructure.Interfaces;
using BusRouteControl.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusRouteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBusRouteRepository _busRouteRepository;

        public BusRouteController(IUnitOfWork unitOfWork, IBusRouteRepository busRouteRepository)
        {
            _unitOfWork = unitOfWork;
            _busRouteRepository = busRouteRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.BusRoutes.GetAll());
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetBusRoute(int id)
        {
            return Ok(await _unitOfWork.BusRoutes.GetBusRouteById(id));
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
            model = await _unitOfWork.BusRoutes.Create(model);
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
            var success = await _unitOfWork.BusRoutes.UpdateBusRoute(id, model);
            if (!success)
                return NotFound();

            return Ok(new { message = "Bus route updated successfully." });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBusRoute(int id)
        {
            var success = await _unitOfWork.BusRoutes.DeleteBusRoute(id);
            if (!success)
                return NotFound();

            return Ok(new { message = "Bus route deleted successfully." });
        }
    }
}
