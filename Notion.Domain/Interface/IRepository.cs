using System.Linq.Expressions;
using Notion.Domain.Entities;

namespace Notion.Domain.Interface
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task CreateAsync(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, object>> sortBy,
            int pageNumber,
            int pageSize,
            bool ascending);
        
        Task<TEntity?> GetByIdAsync(string id);
        
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> condition);
        
        Task UpdateAsync(TEntity entity);
        
        Task DeleteAsync(string id);
    }
}
