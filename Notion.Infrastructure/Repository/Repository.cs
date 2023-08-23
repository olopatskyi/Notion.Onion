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
        private readonly IMongoCollection<TEntity> _collection;

        public Repository(IMongoClient client, DatabaseSettings databaseSettings)
        {
            var database = client.GetDatabase(databaseSettings.Database);
            _collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, object>> sortBy,
            int pageNumber,
            int pageSize,
            bool ascending)
        {
            var query = _collection.Find(filter);

            if (ascending)
            {
                query = query.SortBy(sortBy);
            }
            else
            {
                query = query.SortByDescending(sortBy);
            }

            int totalItems = (int)await query.CountDocumentsAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            int skip = (pageNumber - 1) * pageSize;

            var paginatedItems = await query.Skip(skip).Limit(pageSize).ToListAsync();

            return paginatedItems;
        }

        public async Task<TEntity?> GetByIdAsync(string id)
        {
            return await _collection.Find(e => e.Id == new ObjectId(id)).FirstOrDefaultAsync();
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await _collection.Find(condition).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(e => e.Id == new ObjectId(id));
        }
    }
}