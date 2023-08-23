namespace Notion.Application.Models.Request;

public class CreateToDoList
{
    public string Title { get; set; } = null!;

    public string OwnerId { get; set; } = null!;
    
    public IEnumerable<string>? UserIds { get; set; }

    public IEnumerable<CreateToDoItem>? ToDoItems { get; set; }
}