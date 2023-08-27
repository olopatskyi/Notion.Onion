using MongoDB.Bson;

namespace Notion.Domain.Entities;

public class Contributor : BaseEntity
{
    private List<Permission> _permissions = new();
    
    public string UserId { get; set; } = null!;

    public ObjectId ListId { get; set; }

    public List<Permission> Permissions
    {
        get => _permissions;
        set => _permissions = value;
    }
}