using MongoDB.Bson;

namespace Notion.Domain.Entities;

public abstract class BaseEntity
{
    public ObjectId Id { get; set; }
}