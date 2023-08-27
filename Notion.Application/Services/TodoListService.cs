using System.Linq.Expressions;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Notion.Application.EventHandlers.ToDoListEventHandler;
using Notion.Application.Helpers;
using Notion.Application.Interfaces;
using Notion.Application.Models.Request;
using Notion.Application.Models.Response;
using Notion.Domain.Entities;
using Notion.Domain.Exceptions;
using Notion.Domain.Interface;
using Notion.Domain.Shared;

namespace Notion.Application.Services;

public class TodoListService : ITodoListService
{
    private readonly IMapper _mapper;
    private readonly IToDoListRepository _toDoListRepository;
    private readonly IRepository<Contributor> _contributorRepository;
    private readonly ToDoListEventHandler _eventHandler;

    public TodoListService(IMapper mapper, IToDoListRepository toDoListRepository, ToDoListEventHandler eventHandler,
        IRepository<Contributor> contributorRepository)
    {
        _mapper = mapper;
        _toDoListRepository = toDoListRepository;
        _eventHandler = eventHandler;
        _contributorRepository = contributorRepository;
    }

    public async Task CreateAsync(string userId, CreateToDoList model)
    {
        var entity = _mapper.Map<ToDoList>(model);

        //Here should be HEAD request on identity server for check if user exists
        entity.OwnerId = userId;

        await _toDoListRepository.CreateAsync(entity);
        _eventHandler.CreateInvoke(new ToDoListEventArgs()
        {
            UserId = userId,
            ListId = entity.Id.ToString()
        });
    }

    public async Task<IEnumerable<GetAllToDoListResponse>> GetAsync(string userId, GetToDoLists model)
    {
        var contributors = await _contributorRepository.GetByConditionAsync(x => x.UserId == userId);
        var listIds = contributors.Select(x => x.ListId);
        var filterDefinition = Builders<ToDoList>.Filter.In("_id", listIds);
        var sortDefinition = GetSortDefinition(model.SortBy, model.OrderBy);

        var lists = await _toDoListRepository.GetByFilterDefinitionAsync(
            filterDefinition,
            sortDefinition,
            model.PageSize,
            model.PageNumber
        );


        return _mapper.Map<IEnumerable<GetAllToDoListResponse>>(lists);
    }

    public async Task<GetToDoListResponse> GetByIdAsync(string userId, string listId)
    {
        var contributor =
            await _contributorRepository.FindAsync(x => x.UserId == userId && x.ListId == new ObjectId(listId));
        if (contributor == null)
        {
            throw NotFoundException.Default(contributor);
        }

        if (!PermissionHelper.HasPermission(contributor.Permissions, Permissions.CanReadItems))
        {
            throw ForbiddenException.Default();
        }
        
        var toDoList = await _toDoListRepository.GetByIdAsync(listId)
                       ?? throw NotFoundException.Default(listId);


        return _mapper.Map<GetToDoListResponse>(toDoList);
    }

    public async Task DeleteAsync(string userId, string toDoListId)
    {
        var todoList = await _toDoListRepository.GetByIdAsync(toDoListId);
        if (todoList == null)
        {
            throw new NotFoundException($"{nameof(ToDoList)} not found");
        }

        if (todoList.OwnerId != userId)
        {
            throw ForbiddenException.Default();
        }

        await _toDoListRepository.DeleteAsync(toDoListId);
    }

    public Task UpdateAsync(string userId, UpdateToDoList model)
    {
        throw new NotImplementedException();
    }

    public async Task AddContributorAsync(string userId, string listId, AddContributorRequest model)
    {
        var contributor =
            await _contributorRepository.FindAsync(x => x.UserId == userId && x.ListId == new ObjectId(listId));
        if (contributor == null)
        {
            throw ForbiddenException.Default();
        }

        if (!PermissionHelper.HasPermission(contributor.Permissions, Permissions.CanAddContributor))
        {
            throw ForbiddenException.Default();
        }

        var newContributor = new Contributor()
        {
            ListId = new ObjectId(listId),
            UserId = model.ContributorId,
            Permissions = model.GetSelectedPermissions()
        };

        await _contributorRepository.CreateAsync(newContributor);
    }

    private SortDefinition<ToDoList> GetSortDefinition(string? sortBy, OrderBy orderBy)
    {
        if (string.IsNullOrEmpty(sortBy))
        {
            sortBy = "_id";
        }

        var sortField = Char.ToUpper(sortBy[0]) + sortBy.Substring(1); // Capitalize first letter


        return orderBy == OrderBy.Ascending
            ? Builders<ToDoList>.Sort.Ascending(sortField)
            : Builders<ToDoList>.Sort.Descending(sortField);
    }
}