﻿using BusRouteControl.Domain.Dtos;
using BusRouteControl.Infrastructure.Contracts;
using BusRouteControl.Infrastructure.Interfaces;
using BusRouteControl.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _unitOfWork.Users.GetAllUsersWithTicketsAsync();

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
            var user = await _unitOfWork.Users.GetUserWithTicketsByIdAsync(id);
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

            var userId = await _unitOfWork.Users.CreateUserAsync(model);
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

            var updated = await _unitOfWork.Users.UpdateUserAsync(model);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _unitOfWork.Users.DeleteUserAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);

            if (user == null || user.Password != loginDto.Password || user.Role != loginDto.Role)
                return Unauthorized("Invalid credentials.");

            return Ok(new
            {
                Email = user.Email,
                Role = user.Role
            });
        }

    }
}