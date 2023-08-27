using Notion.Domain.Entities;

namespace Notion.Application.Models.Response;

public class GetToDoListResponse
{
    public string Id { get; set; } = null!;
    
    public string Title { get; set; } = null!;

    public string OwnerId { get; set; } = null!;

    public IEnumerable<ToDoItem> Items { get; set; } = null!;

    public IEnumerable<string> UserIds { get; set; } = null!;

}