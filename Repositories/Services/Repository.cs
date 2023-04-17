using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using bloggers.Repositories.Interfaces;

namespace bloggers.Repositories.Services
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class 
    {

        protected readonly DbContext _Context;      

        public Repository(DbContext Context)
        {
            _Context = Context;
        }  


        public async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> expression){
            return await _Context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return  await _Context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _Context.Set<TEntity>().FindAsync(id) ?? null!;
        }

        public void Remove(TEntity entity)
        {
            _Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _Context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _Context.Set<TEntity>().SingleOrDefaultAsync(predicate) ?? null!;;
        }

        public async Task<int> CountWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return await _Context.Set<TEntity>().CountAsync(predicate);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _Context.Set<TEntity>().AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null!, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!, params string[] includeProperties)
        {
            IQueryable<TEntity> query = _Context.Set<TEntity>();
    
            if (filter != null)
            {
                query = query.Where(filter);
            }
    
            if (includeProperties.Length > 0)
            {
                query = includeProperties.Aggregate(query, (theQuery, theInclude) => theQuery.Include(theInclude));
            }
    
            if (orderBy != null)
            {
                query = orderBy(query);
            }
    
            return await query.ToListAsync();
        }

        public void Update(TEntity entity, string excludeProperties = null!){
            _Context.Set<TEntity>().Attach(entity);
            _Context.Entry(entity).State = EntityState.Modified;
            if(!string.IsNullOrEmpty(excludeProperties)){
                string[] excludingProperties = excludeProperties.Split(",");
                foreach(var property in excludingProperties){
                    _Context.Entry(entity).Property(property).IsModified = false;
                }
            }
        }
    }
}