using Microsoft.AspNetCore.Mvc;
using Notion.Application.Interfaces;
using Notion.Application.Models.Request;

namespace Notion.WebApi.Controllers;

[ApiController]
[Route("api/v1/todolist")]
public class ToDoListController : ControllerBase
{
    private readonly ITodoListService _todoListService;

    public ToDoListController(ITodoListService todoListService)
    {
        _todoListService = todoListService;
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> CreateAsync(string userId, [FromBody] CreateToDoList model)
    {
        var ownerId = Guid.NewGuid().ToString();

        await _todoListService.CreateAsync(userId, model);

        return Ok();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAllAsync(string userId, [FromQuery] GetToDoLists model)
    {
        var lists = await _todoListService.GetAsync(userId, model);
        return Ok(lists);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByIdAsync(string userId, string id)
    {
        var toDoList = await _todoListService.GetByIdAsync(userId,id);

        return Ok(toDoList);
    }
}