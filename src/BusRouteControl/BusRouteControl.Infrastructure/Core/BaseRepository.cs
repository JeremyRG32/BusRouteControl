using BusRouteControl.Domain.Core;
using BusRouteControl.Infrastructure.Context;
using BusRouteControl.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.Infrastructure.Core
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly BusRouteControlDbContext Context;
        protected readonly DbSet<T> DbSet;

        public BaseRepository(BusRouteControlDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public async Task<T> GetEntityById(int id)
        {
            return await DbSet.FindAsync(id);
        }
        public async Task<int> AddEntity(T entity)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity.Id;
        }
        public async Task<bool> DeleteEntity(int id)
        {
            var entity = await GetEntityById(id);
            if (entity == null)
                return false;

            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateEntity(T entity)
        {
            Context.Update(entity);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
