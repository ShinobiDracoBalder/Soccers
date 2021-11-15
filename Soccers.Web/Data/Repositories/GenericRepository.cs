using Microsoft.EntityFrameworkCore;
using Soccers.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soccers.Web.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly DataContext _dataContext;

        public GenericRepository(DataContext dataContext){
            _dataContext = dataContext;
            _dbSet = _dataContext.Set<TEntity>();
        }
        

        public TEntity Get(int id) => _dbSet.Find(id);


        public IEnumerable<TEntity> GetAll() => _dbSet.ToList();
        
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
            await SaveAllAsync();
            return entity;
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await SaveAllAsync();
            return entity;
        }

        public async Task<Response> DeleteAsync(TEntity entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await SaveAllAsync();
                return new Response {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<TEntity> GetAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<bool> SaveAllAsync()
        {
            return await this._dataContext.SaveChangesAsync() > 0;
        }
    }
}
