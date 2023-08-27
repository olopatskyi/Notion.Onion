using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Notion.Application.Interfaces;
using Notion.Application.Models.Request;

namespace Notion.WebApi.Controllers;

[Route("api/{userId}/todolist/{listId}/todoitems")]
[ApiController]
public class ToDoItemController : ControllerBase
{
    private readonly ITodoItemService _todoItemService;

    public ToDoItemController(ITodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(string userId, string listId, [FromBody] CreateToDoItem model)
    {
        await _todoItemService.CreateAsync(userId, listId, model);
        return Ok();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAsync(string userId, string listId, string id,
        [FromBody] UpdateToDoItem model)
    {
        await _todoItemService.UpdateAsync(userId, listId, id, model);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateItemsAsync(string userId, string listId,
        [FromBody] List<UpdateToDoItemsRequest> model)
    {
        await _todoItemService.UpdateToDoItemsAsync(userId, listId, model);
        return Ok();
    }
}