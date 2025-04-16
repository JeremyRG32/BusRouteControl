using BusRouteControl.Application.Contracts;
using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Contracts;
using BusRouteControl.Infrastructure.Models;

namespace BusRouteControl.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<User>> GetAllUsersWithTicketsAsync()
        {
            return await _unitOfWork.Users.GetAllUsersWithTicketsAsync();
        }

        public async Task<User?> GetUserWithTicketsByIdAsync(int id)
        {
            return await _unitOfWork.Users.GetUserWithTicketsByIdAsync(id);
        }

        public async Task<int> CreateUserAsync(UserModel model)
        {
            var userId = await _unitOfWork.Users.CreateUserAsync(model);
            await _unitOfWork.SaveAsync();
            return userId;
        }

        public async Task<bool> UpdateUserAsync(UserModel model)
        {
            var updated = await _unitOfWork.Users.UpdateUserAsync(model);
            if (!updated)
                return false;

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var deleted = await _unitOfWork.Users.DeleteUserAsync(id);
            if (!deleted)
                return false;

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _unitOfWork.Users.GetByEmailAsync(email);
        }
    }
}
