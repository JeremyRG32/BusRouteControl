using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Models;

namespace BusRouteControl.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CreateUserAsync(UserModel model);
        Task<bool> DeleteUserAsync(int id);
        Task<List<User>> GetAllUsersWithTicketsAsync();
        Task<User?> GetUserWithTicketsByIdAsync(int id);
        Task<bool> UpdateUserAsync(UserModel model);
        Task<User> GetByEmailAsync(string email);
    }
}