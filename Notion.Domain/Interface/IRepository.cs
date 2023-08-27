using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using Notion.Domain.Entities;

namespace Notion.Domain.Interface
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task CreateAsync(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, object>>? sortBy,
            int pageNumber,
            int pageSize,
            bool ascending);

        Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> condition);
        
        Task<TEntity?> GetByIdAsync(string id);
        
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> condition);
        
        Task<UpdateResult> UpdateAsync(string id, UpdateDefinition<TEntity> updateDefinition);

        Task<UpdateResult> UpdateAsync(FilterDefinition<TEntity> filterDefinition,
            UpdateDefinition<TEntity> updateDefinition);
        
        Task DeleteAsync(string id);
    }
}
