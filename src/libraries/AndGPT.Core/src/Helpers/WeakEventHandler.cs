using System.Reflection;

namespace AndGPT.Core.Helpers;

public class WeakEventHandler<TEventArgs>(EventHandler<TEventArgs> handler)
    where TEventArgs : EventArgs
{
    private readonly WeakReference _targetRef = new(handler.Target);
    private readonly Action<object, TEventArgs> _method = (Action<object, TEventArgs>)handler.Method.CreateDelegate(typeof(Action<object, TEventArgs>), handler.Target);

    public void Handler(object sender, TEventArgs e)
    {
        if (_targetRef.IsAlive)
        {
            _method(sender, e);
        }
    }

    public EventHandler<TEventArgs> HandlerProxy => Handler!;
}

