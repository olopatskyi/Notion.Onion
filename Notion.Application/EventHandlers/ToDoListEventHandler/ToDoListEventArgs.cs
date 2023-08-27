namespace Notion.Application.EventHandlers.ToDoListEventHandler;

public class ToDoListEventArgs : EventArgs
{
    public string UserId { get; set; } = null!;

    public string ListId { get; set; } = null!;
}