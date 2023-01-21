using System;
using System.Linq.Expressions;
using backend_engine.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_engine.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        public readonly BreezeDataContext _context;

        public readonly DbSet<TEntity> _entities;

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

        public virtual async Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public virtual async Task<TEntity> GetById(int Id)
        {
            return await _entities.SingleOrDefaultAsync(q => q.Id == Id);
        }

        public virtual async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public virtual async Task<bool> RemoveEntity(int Id)
        {
            TEntity entity = await _entities.SingleOrDefaultAsync(q => q.Id == Id);
            _context.Remove(entity);

            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;


        }

        public void UpdateEntity(TEntity entity)
        {

            _context.Entry(entity).State = EntityState.Modified;

        }

        TEntity IRepository<TEntity>.UpdateEntity(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool EntityExists(int Id)
        {

            return (_entities?.Any(e => e.Id == Id)).GetValueOrDefault();
        }
    }
}

