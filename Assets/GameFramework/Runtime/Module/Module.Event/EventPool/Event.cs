namespace GameFramework.Module.Event
{
    /// <summary>
    /// 事件结点。
    /// </summary>
    public sealed class Event<T> : IReference where T : BaseEventArgs
    {
        private object _sender;
        private T _eventArgs;

        public Event()
        {
            _sender = null;
            _eventArgs = null;
        }

        public object Sender => _sender;

        public T EventArgs => _eventArgs;

        public static Event<T> Create(object sender, T e)
        {
            var eventNode = ReferencePool.Acquire<Event<T>>();
            eventNode._sender = sender;
            eventNode._eventArgs = e;
            return eventNode;
        }

        public void Clear()
        {
            _sender = null;
            _eventArgs = null;
        }
    }
}