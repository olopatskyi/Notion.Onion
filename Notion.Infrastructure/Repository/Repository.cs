using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using Notion.Domain.Entities;
using Notion.Domain.Interface;
using Notion.Domain.Shared;

namespace Notion.Infrastructure.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IMongoCollection<TEntity> Collection;

        public Repository(IMongoClient client, DatabaseSettings databaseSettings)
        {
            var database = client.GetDatabase(databaseSettings.Database);
            Collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task CreateAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, object>>? sortBy,
            int pageNumber,
            int pageSize,
            bool ascending)
        {
            var query = Collection.Find(filter);

            if (sortBy != null)
            {
                if (ascending)
                {
                    query = query.SortBy(sortBy);
                }
                else
                {
                    query = query.SortByDescending(sortBy);
                }
            }

            int totalItems = (int)await query.CountDocumentsAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            int skip = (pageNumber - 1) * pageSize;

            var paginatedItems = await query.Skip(skip).Limit(pageSize).ToListAsync();

            return paginatedItems;
        }

        public async Task<TEntity?> GetByIdAsync(string id)
        {
            return await Collection.Find(e => e.Id == new ObjectId(id)).FirstOrDefaultAsync();
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await Collection.Find(condition).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await Collection.DeleteOneAsync(e => e.Id == new ObjectId(id));
        }
    }
}