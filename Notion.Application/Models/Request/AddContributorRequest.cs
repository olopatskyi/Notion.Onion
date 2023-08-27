using Notion.Domain.Entities;
using Notion.Domain.Shared;

namespace Notion.Application.Models.Request;

public class AddContributorRequest
{
    public string ContributorId { get; set; } = null!;

    public bool CanReadItems { get; set; }

    public bool CanCreateItem { get; set; }

    public bool CanUpdateItem { get; set; }

    public bool CanDeleteItem { get; set; }

    public bool CanAddContributor { get; set; }

    public bool CanDeleteContributor { get; set; }


    public List<Permission> GetSelectedPermissions()
    {
        var selectedPermissions = new List<Permission>
        {
            new Permission { Action = Permissions.CanReadItems, CanPerform = CanReadItems },
            new Permission { Action = Permissions.CanCreateItem, CanPerform = CanCreateItem },
            new Permission { Action = Permissions.CanUpdateItem, CanPerform = CanUpdateItem },
            new Permission { Action = Permissions.CanDeleteItem, CanPerform = CanDeleteItem },
            new Permission { Action = Permissions.CanAddContributor, CanPerform = CanAddContributor },
            new Permission { Action = Permissions.CanDeleteContributor, CanPerform = CanDeleteContributor }
        };

        return selectedPermissions;
    }
}