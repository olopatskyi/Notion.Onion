namespace Notion.Application.EventHandlers;

public class BaseEventHandler<TArgs> where TArgs : EventArgs
{
    public event EventHandler<TArgs> OnCreate;

    public void CreateInvoke(TArgs args)
    {
        OnCreate?.Invoke(this, args);
    }
}