using BusRouteControl.Domain.Dtos;
using BusRouteControl.Infrastructure.Models;
using BusRouteControl.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userRepository.GetAllUsersWithTicketsAsync();

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
            var user = await _userRepository.GetUserWithTicketsByIdAsync(id);
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

            var userId = await _userRepository.CreateUserAsync(model);
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

            var updated = await _userRepository.UpdateUserAsync(model);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userRepository.DeleteUserAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}