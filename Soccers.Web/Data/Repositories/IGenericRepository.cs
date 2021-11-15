using Soccers.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Soccers.Web.Data.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(int id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<Response> DeleteAsync(TEntity entity);
    }
}
