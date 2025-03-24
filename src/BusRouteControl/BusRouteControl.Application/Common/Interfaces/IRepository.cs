using System.Linq.Expressions;

namespace BusRouteControl.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        IQueryable<T> Get(Expression<Func<T, bool>>? filter, string? includeProperties = null);
        void Add(T busRoute);
        void Remove(T busRoute);
        Task AddAsync(T entity);
        Task<IQueryable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
