using BusRouteControl.Domain.Dtos;
using BusRouteControl.Infrastructure.Contracts;
using BusRouteControl.Infrastructure.Interfaces;
using BusRouteControl.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketController(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _ticketRepository = ticketRepository;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            var tickets = await _unitOfWork.Tickets.GetAllTicketsAsync();

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
            var ticket = await _unitOfWork.Tickets.GetTicketByIdAsync(id);

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

            var newId = await _unitOfWork.Tickets.CreateTicketAsync(ticket);

            dto.Id = newId;

            return CreatedAtAction("GetTicket", new { id = newId }, dto);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateTicket(int id, TicketDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var ticket = await _unitOfWork.Tickets.GetTicketByIdAsync(id);
            if (ticket == null)
                return NotFound();

            var model = new TicketModel
            {
                Id = ticket.Id,
                UserId = dto.UserId,
                ScheduleId = dto.ScheduleId,
                Price = dto.Price,
                Status = dto.Status,
                BookingDate = dto.BookingDate
            };

            await _unitOfWork.Tickets.UpdateTicketAsync(model);

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var deleted = await _unitOfWork.Tickets.DeleteTicketAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}