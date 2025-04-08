using BusRouteControl.API.Dtos;
using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly BusRouteControlDbContext _context;

        public TicketController(BusRouteControlDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            var tickets = await _context.Tickets.ToListAsync();

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
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

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
        public async Task<ActionResult<TicketDto>> CreateTicket(TicketDto ticketDTO)
        {
            var ticket = new Ticket
            {
                UserId = ticketDTO.UserId,
                ScheduleId = ticketDTO.ScheduleId,
                Price = ticketDTO.Price,
                Status = ticketDTO.Status,
                BookingDate = ticketDTO.BookingDate
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            var createdTicketDTO = new TicketDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                ScheduleId = ticket.ScheduleId,
                Price = ticket.Price,
                Status = ticket.Status,
                BookingDate = ticket.BookingDate
            };

            return CreatedAtAction("GetTicket", new { id = ticket.Id }, createdTicketDTO);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateTicket(int id, TicketDto ticketDTO)
        {
            if (id != ticketDTO.Id)
            {
                return BadRequest();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ticket.UserId = ticketDTO.UserId;
            ticket.ScheduleId = ticketDTO.ScheduleId;
            ticket.Price = ticketDTO.Price;
            ticket.Status = ticketDTO.Status;
            ticket.BookingDate = ticketDTO.BookingDate;

            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
