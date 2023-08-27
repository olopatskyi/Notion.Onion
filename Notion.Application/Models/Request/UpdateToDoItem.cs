namespace Notion.Application.Models.Request;

public class UpdateToDoItem
{
    public string? Title { get; set; }
    
    public bool? Completed { get; set; }
}