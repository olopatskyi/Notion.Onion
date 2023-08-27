using MongoDB.Bson;

namespace Notion.Domain.Entities;

public class ToDoItem : BaseEntity
{
    public string Title { get; set; } = null!;
    
    public bool Completed { get; set; }
}