namespace Notion.Domain.Entities;

public class ToDoList : BaseEntity
{
    public ToDoList()
    {
        Items = new List<ToDoItem>();
    }

    public string Title { get; set; } = null!;
    public string OwnerId { get; set; } = null!;
    public ICollection<ToDoItem> Items { get; set; }
}