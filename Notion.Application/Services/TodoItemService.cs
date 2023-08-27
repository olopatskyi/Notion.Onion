using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Notion.Application.Helpers;
using Notion.Application.Interfaces;
using Notion.Application.Models.Request;
using Notion.Domain.Entities;
using Notion.Domain.Exceptions;
using Notion.Domain.Interface;
using Notion.Domain.Shared;

namespace Notion.Application.Services;

public class TodoItemService : ITodoItemService
{
    private readonly IRepository<ToDoList> _todoListRepository;
    private readonly IRepository<Contributor> _contributorRepository;
    private readonly IMapper _mapper;

    public TodoItemService(IRepository<ToDoList> todoListRepository, IMapper mapper,
        IRepository<Contributor> contributorRepository)
    {
        _todoListRepository = todoListRepository;
        _mapper = mapper;
        _contributorRepository = contributorRepository;
    }

    public async Task CreateAsync(string userId, string toDoListId, CreateToDoItem model)
    {
        var contributor =
            await _contributorRepository.FindAsync(x => x.UserId == userId && x.ListId == new ObjectId(toDoListId));

        if (contributor == null)
        {
            throw ForbiddenException.Default();
        }

        if (!PermissionHelper.HasPermission(contributor.Permissions, Permissions.CanCreateItem))
        {
            throw ForbiddenException.Default();
        }

        var toDoList = await _todoListRepository.GetByIdAsync(toDoListId);
        if (toDoList == null)
        {
            throw NotFoundException.Default(toDoList);
        }

        var todoItem = _mapper.Map<ToDoItem>(model);

        toDoList.Items.Add(todoItem);

        var updateDefinition = Builders<ToDoList>.Update.Push(x => x.Items, todoItem);

        var result = await _todoListRepository.UpdateAsync(toDoList.Id, updateDefinition);
        if (result.ModifiedCount == 0)
        {
            throw new UnhandledException("Entity was not updated");
        }
    }

    public async Task UpdateAsync(string userId, string toDoListId, string title, UpdateToDoItem model)
    {
        var toDoList = await _todoListRepository.GetByIdAsync(toDoListId);
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

    public async Task UpdateToDoItemsAsync(string userId, string listId, List<UpdateToDoItemsRequest> model)
    {
        var contributor = await _contributorRepository.FindAsync(x => x.UserId == userId && x.ListId == new ObjectId(listId));
        if (contributor == null)
        {
            throw ForbiddenException.Default();
        }

        if (!PermissionHelper.HasPermission(contributor.Permissions, Permissions.CanUpdateItem))
        {
            throw ForbiddenException.Default();
        }
        
        var filter = Builders<ToDoList>.Filter.Eq("_id", new ObjectId(listId));
        var toDoList = await _todoListRepository.FindAsync(x => x.Id == listId);
        if (toDoList == null)
        {
           throw NotFoundException.Default(toDoList);
        }

        // Update changed items and remove missing items
        foreach (var dbItem in toDoList.Items.ToList())
        {
            var updatedItem = model.FirstOrDefault(updated => updated.Id == dbItem.Id);
            if (updatedItem != null)
            {
                dbItem.Title = updatedItem.Title;
                dbItem.Completed = updatedItem.Completed;
            }
            else
            {
                // Item is missing in the updated list, remove it from the database list
                toDoList.Items.Remove(dbItem);
            }
        }


        // Add new items to the database list
        foreach (var updatedItem in model.Where(updatedItem => toDoList.Items.All(dbItem => dbItem.Id != updatedItem.Id)))
        {
            toDoList.Items.Add(_mapper.Map<ToDoItem>(updatedItem));
        }

        var update = Builders<ToDoList>.Update
            .Set(x => x.Items, toDoList.Items);

        await _todoListRepository.UpdateAsync(filter, update);
    }

    private void UpdateToDoItem(ToDoItem toDoItem, UpdateToDoItem model)
    {
        if (model.Title != null)
        {
            toDoItem.Title = model.Title;
        }

        if (model.Completed != null)
        {
            toDoItem.Completed = model.Completed.Value;
        }
    }
}