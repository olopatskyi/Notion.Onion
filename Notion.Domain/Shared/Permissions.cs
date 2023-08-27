namespace Notion.Domain.Shared;

public static class Permissions
{
    public const string CanReadItems = "get:items";
    public const string CanCreateItem = "create:item";
    public const string CanUpdateItem = "update:item";
    public const string CanDeleteItem = "delete:item";
    public const string CanAddContributor = "create:contributor";
    public const string CanDeleteContributor = "delete:contributor";

    public static List<string> Admin = new(new[]
    {
        CanReadItems,
        CanCreateItem,
        CanUpdateItem,
        CanDeleteItem,
        CanAddContributor,
        CanDeleteContributor
    });
}