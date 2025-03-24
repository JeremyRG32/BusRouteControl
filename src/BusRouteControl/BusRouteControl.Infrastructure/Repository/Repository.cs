using BusRouteControl.Application.Common.Interfaces;
using BusRouteControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusRouteControl.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BusRouteControlDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(BusRouteControlDbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }
        public void Add(T busRoute)
        {
            dbSet.Add(busRoute);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query;
        }

        public void Remove(T busRoute)
        {
            dbSet.Remove(busRoute);
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task<IQueryable<T>> GetAllAsync()
        {
            return _context.Set<T>();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
