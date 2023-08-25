using AutoMapper;
using Notion.Application.Interfaces;
using Notion.Application.Models.Request;
using Notion.Domain.Entities;
using Notion.Domain.Exceptions;
using Notion.Domain.Interface;

namespace Notion.Application.Services;

public class TodoItemService : ITodoItemService
{
    private readonly IRepository<ToDoList> _repository;
    private readonly IMapper _mapper;

    public TodoItemService(IRepository<ToDoList> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateAsync(string userId, string toDoListId, CreateToDoItem model)
    {
        var toDoList = await _repository.GetByIdAsync(toDoListId);
        if (toDoList == null)
        {
            throw NotFoundException.Default(toDoList);
        }

        if (toDoList.OwnerId != userId)
        {
            throw ForbiddenException.Default();
        }

        var todoItem = _mapper.Map<ToDoItem>(model);

        toDoList.Items?.Add(todoItem);

        await _repository.UpdateAsync(toDoList);
    }

    public async Task UpdateAsync(string userId, string toDoListId, string title, UpdateToDoItem model)
    {
        var toDoList = await _repository.GetByIdAsync(toDoListId);
        if (toDoList == null)
        {
            throw NotFoundException.Default(toDoList);
        }

        if (userId != toDoList.OwnerId)
        {
            throw ForbiddenException.Default();
        }

        var toDoItem = toDoList.Items?.FirstOrDefault(x => x.Title == title);
        if (toDoItem == null)
        {
            throw NotFoundException.Default(toDoItem);
        }
        
        UpdateToDoItem(toDoItem, model);
        
        //Logic to update todoitem in collection within document
    }

    private void UpdateToDoItem(ToDoItem toDoItem, UpdateToDoItem model)
    {
        if (model.Title != null)
        {
            toDoItem.Title = model.Title;
        }
        
        if (model.Description != null)
        {
            toDoItem.Description = model.Description;
        }
        
        if (model.Completed != null)
        {
            toDoItem.Completed = model.Completed.Value;
        }
    }
}