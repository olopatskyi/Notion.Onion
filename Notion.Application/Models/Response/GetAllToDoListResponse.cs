namespace Notion.Application.Models.Response;

public class GetAllToDoListResponse
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public IEnumerable<string>? Contributors { get; set; }
}