namespace Notion.Application.EventHandlers.ToDoListEventHandler;

public class ToDoListEventHandler : BaseEventHandler<ToDoListEventArgs>
{
    private readonly ToDoListObserver _listObserver;

    public ToDoListEventHandler(ToDoListObserver listObserver)
    {
        _listObserver = listObserver;

        OnCreate += _listObserver.CreateContributor;
    }
}