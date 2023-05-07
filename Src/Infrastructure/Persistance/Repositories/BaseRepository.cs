using System;
using System.Linq.Expressions;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories
{
	public class BaseRepository<T> : IRepository<T> where T : class
	{
        protected readonly WgDbContext _context;
        private readonly DbSet<T> _objectSet;
        public BaseRepository(WgDbContext context)
        {
            _context = context;
            _objectSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _objectSet.AddAsync(entity);
        }

        public Task Delete(T entity)
        {
            _objectSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(
                query,
                (current, includeProperties) => current.Include(includeProperties));
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _objectSet.FindAsync(id);
        }
    }
}

