namespace Notion.Domain.Entities;

public class Permission
{
    public string Action { get; set; } = null!;

    public bool CanPerform { get; set; }
}