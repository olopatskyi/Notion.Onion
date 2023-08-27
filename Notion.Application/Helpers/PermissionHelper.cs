using Notion.Domain.Entities;

namespace Notion.Application.Helpers;

public static class PermissionHelper
{
    public static bool HasPermission(IEnumerable<Permission> permissions, string action)
    {
        return permissions.Any(p => p.Action == action && p.CanPerform);
    }
}