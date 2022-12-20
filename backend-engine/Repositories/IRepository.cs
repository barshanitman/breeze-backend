using System;
using System.Linq.Expressions;
using backend_engine.Models;

namespace backend_engine.Repositories
{
	public interface IRepository<TEntity>
	{
		Task<IEnumerable<TEntity>> GetAll();

		Task<TEntity> GetById(int Id);

		TEntity Add(TEntity entity);

		Task<bool> SaveChanges();

		Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate);
        Task<bool> RemoveEntity(int id);
    }
}

