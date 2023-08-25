namespace Notion.Application.Models.Request;

public class CreateToDoList
{
    public string Title { get; set; } = null!;
    
    public IEnumerable<string>? Contributors { get; set; }

    public IEnumerable<CreateToDoItem>? ToDoItems { get; set; }
}