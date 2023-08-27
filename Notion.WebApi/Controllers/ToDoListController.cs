using Microsoft.AspNetCore.Mvc;
using Notion.Application.Interfaces;
using Notion.Application.Models.Request;

namespace Notion.WebApi.Controllers;

[ApiController]
[Route("api/v1/{userId}/todolist")]
public class ToDoListController : ControllerBase
{
    private readonly ITodoListService _todoListService;

    public ToDoListController(ITodoListService todoListService)
    {
        _todoListService = todoListService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(string userId, [FromBody] CreateToDoList model)
    {
        await _todoListService.CreateAsync(userId, model);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(string userId, [FromQuery] GetToDoLists model)
    {
        var lists = await _todoListService.GetAsync(userId, model);
        return Ok(lists);
    }

    [HttpGet("{listId}")]
    public async Task<IActionResult> GetByIdAsync(string userId, string listId)
    {
        var toDoList = await _todoListService.GetByIdAsync(userId,listId);

        return Ok(toDoList);
    }

    [HttpPost("{listId}/contributors")]
    public async Task<IActionResult> AddContributorAsync(string userId, string listId, [FromBody] AddContributorRequest model)
    {
        await _todoListService.AddContributorAsync(userId, listId, model);
        return Ok();
    }
}