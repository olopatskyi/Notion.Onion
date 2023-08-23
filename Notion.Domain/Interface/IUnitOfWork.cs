using Notion.Domain.Entities;

namespace Notion.Domain.Interface
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
