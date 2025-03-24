using BusRouteControl.Application.DTOs;
using BusRouteControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusRouteController : ControllerBase
    {
        private readonly IRouteService _busRouteService;

        public BusRouteController(IRouteService busRouteService)
        {
            _busRouteService = busRouteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var routes = await _busRouteService.GetAllBusRoutesAsync();
            return Ok(routes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var route = await _busRouteService.GetBusRouteByIdAsync(id);
            if (route == null)
                return NotFound();
            return Ok(route);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusRouteDto routeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newRoute = await _busRouteService.CreateBusRouteAsync(routeDto);
            return CreatedAtAction(nameof(GetById), new { id = newRoute.Id }, newRoute);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusRouteDto routeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _busRouteService.UpdateBusRouteAsync(id, routeDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _busRouteService.DeleteBusRouteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
