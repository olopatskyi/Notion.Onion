namespace Notion.Application.Models.Request;

public class UpdateToDoItemsRequest
{
    public string? Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public bool Completed { get; set; }
}