using System;
using System.Linq.Expressions;
using backend_engine.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_engine.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class,IBaseEntity
    {
        private readonly BreezeDataContext _context;

        private readonly DbSet<TEntity> _entities;

        public Repository(BreezeDataContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            _entities.Add(entity);
            return entity;
        }

        public async Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task<TEntity> GetById(int Id)
        {
            return await _entities.SingleOrDefaultAsync(q => q.Id == Id);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

	public async Task<bool> RemoveEntity(int Id)
	 {
	    TEntity entity = await  _entities.SingleOrDefaultAsync(q => q.Id == Id);
	   _context.Remove(entity);
	   
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
	
	
	    }
    }
}

