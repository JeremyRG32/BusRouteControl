using BusRouteControl.Application.Contracts;
using BusRouteControl.Domain.Dtos;
using BusRouteControl.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersWithTicketsAsync();

            var userDtos = users.Select(u => new UserDto
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
            }).ToList();

            return Ok(userDtos);
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetUserWithTicketsByIdAsync(id);
            if (user == null)
                return NotFound();

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Tickets = user.Tickets.Select(t => new TicketDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Price = t.Price,
                    Status = t.Status,
                    BookingDate = t.BookingDate,
                    ScheduleId = t.ScheduleId
                }).ToList()
            };

            return Ok(userDto);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            var model = new UserModel
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                Role = userDto.Role
            };

            var userId = await _userService.CreateUserAsync(model);
            userDto.Id = userId;

            return CreatedAtAction(nameof(GetUser), new { id = userId }, userDto);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest();

            var model = new UserModel
            {
                Id = userDto.Id,
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                Role = userDto.Role
            };

            var updated = await _userService.UpdateUserAsync(model);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.GetByEmailAsync(loginDto.Email);

            if (user == null || user.Password != loginDto.Password || user.Role != loginDto.Role)
                return Unauthorized("Invalid credentials.");

            return Ok(new
            {
                Email = user.Email,
                Role = user.Role
            });
        }
        [HttpGet("GetByEmail/{email}")]
        public async Task<ActionResult<UserDto>> GetByEmail(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null) return NotFound();
            return Ok(user);
        }

    }
}