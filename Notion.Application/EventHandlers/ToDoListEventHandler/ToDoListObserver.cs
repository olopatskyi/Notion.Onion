using AutoMapper;
using MongoDB.Bson;
using Notion.Domain.Entities;
using Notion.Domain.Interface;
using Notion.Domain.Shared;

namespace Notion.Application.EventHandlers.ToDoListEventHandler;

public class ToDoListObserver
{
    private readonly IRepository<Contributor> _repository;
    private readonly IMapper _mapper;

    public ToDoListObserver(IRepository<Contributor> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async void CreateContributor(object sender, ToDoListEventArgs e)
    {
        var contributor = new Contributor()
        {
            UserId = e.UserId,
            ListId = new ObjectId(e.ListId),
            Permissions = Permissions.Admin.Select(action => new Permission()
            {
                Action = action,
                CanPerform = true
            }).ToList()
        };

        await _repository.CreateAsync(contributor);
    }
}