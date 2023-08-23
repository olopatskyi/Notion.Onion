namespace Notion.Application.Models.Request;

public class CreateToDoItem
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool Completed { get; set; }
}