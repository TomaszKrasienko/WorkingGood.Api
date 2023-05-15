using System;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
	public interface IRepository<T>
	{
		Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
		Task<int> CountAll(params Expression<Func<T, object>>[] includeProperties);
		Task<T> GetByIdAsync(Guid id);
		Task AddAsync(T entity);
		Task Delete(T entity);
	}
}

