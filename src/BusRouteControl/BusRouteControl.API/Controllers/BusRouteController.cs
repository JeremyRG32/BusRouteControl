using BusRouteControl.Application.DTOs;
using BusRouteControl.Application.Services.BusRouteControl.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusRouteController : ControllerBase
    {
        private readonly IBusRouteService _busRouteService;

        public BusRouteController(IBusRouteService busRouteService)
        {
            _busRouteService = busRouteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBusRoute([FromBody] BusRouteDto busRouteDto)
        {
            if (busRouteDto == null)
            {
                return BadRequest("BusRouteDto is null");
            }

            await _busRouteService.CreateBusRouteAsync(busRouteDto);
            return CreatedAtAction(nameof(GetBusRoutes), new { id = busRouteDto.Id }, busRouteDto);
        }

        [HttpGet]
        public async Task<ActionResult<IQueryable<BusRouteDto>>> GetBusRoutes()
        {
            var busRoutes = await _busRouteService.GetAllBusRoutesAsync();
            return Ok(busRoutes);
        }
    }
}
