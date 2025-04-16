using BusRouteControl.Application.Contracts;
using BusRouteControl.Domain.Dtos;
using BusRouteControl.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();

            var ticketDTOs = tickets.Select(ticket => new TicketDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                ScheduleId = ticket.ScheduleId,
                Price = ticket.Price,
                Status = ticket.Status,
                BookingDate = ticket.BookingDate
            }).ToList();

            return Ok(ticketDTOs);
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket == null)
                return NotFound();

            var ticketDTO = new TicketDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                ScheduleId = ticket.ScheduleId,
                Price = ticket.Price,
                Status = ticket.Status,
                BookingDate = ticket.BookingDate
            };

            return Ok(ticketDTO);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<TicketDto>> CreateTicket(TicketDto dto)
        {
            var ticket = new TicketModel
            {
                UserId = dto.UserId,
                ScheduleId = dto.ScheduleId,
                Price = dto.Price,
                Status = dto.Status,
                BookingDate = dto.BookingDate
            };

            var newId = await _ticketService.CreateTicketAsync(ticket);

            dto.Id = newId;

            return CreatedAtAction("GetTicket", new { id = newId }, dto);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateTicket(int id, TicketDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
                return NotFound();

            var model = new TicketModel
            {
                Id = dto.Id,
                UserId = dto.UserId,
                ScheduleId = dto.ScheduleId,
                Price = dto.Price,
                Status = dto.Status,
                BookingDate = dto.BookingDate
            };

            await _ticketService.UpdateTicketAsync(model);

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var deleted = await _ticketService.DeleteTicketAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}