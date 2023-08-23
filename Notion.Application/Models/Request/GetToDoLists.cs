namespace Notion.Application.Models.Request;

public class GetToDoLists
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public string? SortBy { get; set; }

    public OrderBy OrderBy { get; set; }
}