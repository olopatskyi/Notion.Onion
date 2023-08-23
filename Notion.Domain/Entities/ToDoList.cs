namespace Notion.Domain.Entities;

public class ToDoList : BaseEntity
{
    public string Title { get; set; } = null!;

    public string OwnerId { get; set; } = null!;
    
    public ICollection<string>? SharedWithUserIds { get; set; }
    
    public ICollection<ToDoItem>? Items { get; set; }}