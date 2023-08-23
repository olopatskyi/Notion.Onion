namespace Notion.Application.Models.Request;

public class UpdateToDoList
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
}