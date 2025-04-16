using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Models;

namespace BusRouteControl.Application.Contracts
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(UserModel model);
        Task<bool> DeleteUserAsync(int id);
        Task<List<User>> GetAllUsersWithTicketsAsync();
        Task<User> GetByEmailAsync(string email);
        Task<User?> GetUserWithTicketsByIdAsync(int id);
        Task<bool> UpdateUserAsync(UserModel model);
    }
}