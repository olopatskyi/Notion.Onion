using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Notion.Application.Interfaces;
using Notion.Application.Models.Request;

namespace Notion.WebApi.Controllers;

[Route("api/{userId}/todolist")]
[ApiController]
public class ToDoItemController : ControllerBase
{
    private readonly ITodoItemService _todoItemService;

    public ToDoItemController(ITodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    [HttpPost("{listId}/todoItems")]
    public async Task<IActionResult> CreateAsync(string userId, string listId, [FromBody] CreateToDoItem model)
    {
        await _todoItemService.CreateAsync(userId, listId, model);
        return Ok();
    }
    
    [HttpPatch("{listId}/todoItems/{title}")]
    public async Task<IActionResult> UpdateAsync(string userId, string listId, string title, [FromBody] UpdateToDoItem model)
    {
        await _todoItemService.UpdateAsync(userId, listId, title, model);
        return Ok();
    }
}