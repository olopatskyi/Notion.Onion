using System.Linq.Expressions;
using AutoMapper;
using MongoDB.Bson;
using Notion.Application.Interfaces;
using Notion.Application.Models.Request;
using Notion.Domain.Entities;
using Notion.Domain.Exceptions;
using Notion.Domain.Interface;

namespace Notion.Application.Services;

public class TodoListService : ITodoListService
{
    private readonly IMapper _mapper;
    private readonly IRepository<ToDoList> _repository;

    public TodoListService(IMapper mapper, IRepository<ToDoList> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task CreateAsync(string userId, CreateToDoList model)
    {
        var entity = _mapper.Map<ToDoList>(model);

        await _repository.CreateAsync(entity);
    }

    public async Task<IEnumerable<ToDoList>> GetAsync(string userId, GetToDoLists model)
    {
        Expression<Func<ToDoList, object>> sortBy = null;
        Expression<Func<ToDoList, bool>> filter = x => x.OwnerId == userId;

        if (model.SortBy?.ToLower() == nameof(ToDoList.Title).ToLower())
        {
            sortBy = x => x.Title;
        }


        var toDoLists = await _repository.GetAllAsync(filter, sortBy, model.PageNumber, model.PageSize,
            model.OrderBy == OrderBy.Ascending);

        return toDoLists;
    }

    public async Task<ToDoList?> GetByIdAsync(string userId, string toDoListId)
    {
        var toDoList = await _repository.FindAsync(x => x.Id == new ObjectId(toDoListId));
        return toDoList;
    }

    public async Task DeleteAsync(string userId, string toDoListId)
    {
        var todoList = await _repository.GetByIdAsync(toDoListId);
        if (todoList == null)
        {
            throw new NotFoundException($"{nameof(ToDoList)} not found");
        }

        if (todoList.OwnerId != userId)
        {
            throw ForbiddenException.Default();
        }
        await _repository.DeleteAsync(toDoListId);
    }

    public Task UpdateAsync(string userId, UpdateToDoList model)
    {
        throw new NotImplementedException();
    }
}