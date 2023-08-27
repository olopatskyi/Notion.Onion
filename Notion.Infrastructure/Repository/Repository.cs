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

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Collection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter,
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

        public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await Collection.Find(condition).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(string id)
        {
            return await Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await Collection.Find(condition).FirstOrDefaultAsync();
        }

        public async Task<UpdateResult> UpdateAsync(string id, UpdateDefinition<TEntity> updateDefinition)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", new ObjectId(id));
            return await Collection.UpdateOneAsync(filter, updateDefinition);
        }
        
        public async Task<UpdateResult> UpdateAsync(FilterDefinition<TEntity> filterDefinition, UpdateDefinition<TEntity> updateDefinition)
        {
            return await Collection.UpdateOneAsync(filterDefinition, updateDefinition);
        }

        public async Task DeleteAsync(string id)
        {
            await Collection.DeleteOneAsync(e => e.Id == id);
        }
    }
}