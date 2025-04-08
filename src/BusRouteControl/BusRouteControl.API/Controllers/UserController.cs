using BusRouteControl.API.Dtos;
using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BusRouteControlDbContext _context;

        public UserController(BusRouteControlDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Tickets)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Tickets = u.Tickets.Select(t => new TicketDto
                    {
                        Id = t.Id,
                        UserId = t.UserId,
                        Price = t.Price,
                        Status = t.Status,
                        BookingDate = t.BookingDate,
                        ScheduleId = t.ScheduleId
                    }).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<TicketDto>> GetUser(int id)
        {
            var user = await _context.Users
           .Include(u => u.Tickets)
           .Where(u => u.Id == id)
           .Select(u => new UserDto
           {
               Id = u.Id,
               Name = u.Name,
               Email = u.Email,
               Role = u.Role,
               Tickets = u.Tickets.Select(t => new TicketDto
               {
                   Id = t.Id,
                   Price = t.Price,
                   UserId = t.UserId,
                   Status = t.Status,
                   BookingDate = t.BookingDate,
                   ScheduleId = t.ScheduleId
               }).ToList()
           })
           .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Password = userDto.Password,
                Email = userDto.Email,
                Role = userDto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }


        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Role = userDto.Role;
            user.Password = userDto.Password;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
