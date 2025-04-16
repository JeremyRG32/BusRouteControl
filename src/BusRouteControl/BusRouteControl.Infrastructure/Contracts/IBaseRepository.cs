using BusRouteControl.Domain.Core;

namespace BusRouteControl.Infrastructure.Contracts
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<int> AddEntity(T entity);
        Task<bool> DeleteEntity(int id);
        Task<T> GetEntityById(int id);
        Task<bool> UpdateEntity(T entity);
    }
}