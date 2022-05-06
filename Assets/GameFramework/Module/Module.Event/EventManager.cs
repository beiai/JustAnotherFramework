using System;

namespace GameFramework.Module.Event
{
    /// <summary>
    /// 事件管理器。
    /// </summary>
    internal sealed class EventManager : IEventManager, IModule
    {
        private readonly EventPool<GameEventArgs> _eventPool;
        private static EventManager _instance;

        /// <summary>
        /// 初始化事件管理器的新实例。
        /// </summary>
        public EventManager()
        {
            _eventPool = new EventPool<GameEventArgs>(EventPoolMode.AllowNoHandler | EventPoolMode.AllowMultiHandler);
        }

        /// <summary>
        /// 获取事件处理函数的数量。
        /// </summary>
        public int EventHandlerCount => _eventPool.EventHandlerCount;

        /// <summary>
        /// 获取事件数量。
        /// </summary>
        public int EventCount => _eventPool.EventCount;

        public int Priority => 0;

        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    ModuleManager.GetModule<EventManager>();
                }

                return _instance;
            }
        }

        public void OnCreate()
        {
            _instance = this;
        }

        /// <summary>
        /// 事件管理器轮询。
        /// </summary>
        public void Update()
        {
            _eventPool.Update();
        }

        /// <summary>
        /// 关闭并清理事件管理器。
        /// </summary>
        public void Shutdown()
        {
            _eventPool.Shutdown();
        }

        /// <summary>
        /// 获取事件处理函数的数量。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <returns>事件处理函数的数量。</returns>
        public int Count(int id)
        {
            return _eventPool.Count(id);
        }

        /// <summary>
        /// 检查是否存在事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要检查的事件处理函数。</param>
        /// <returns>是否存在事件处理函数。</returns>
        public bool Check(int id, EventHandler<GameEventArgs> handler)
        {
            return _eventPool.Check(id, handler);
        }

        /// <summary>
        /// 订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要订阅的事件处理函数。</param>
        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            _eventPool.Subscribe(id, handler);
        }

        /// <summary>
        /// 取消订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要取消订阅的事件处理函数。</param>
        public void Unsubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            _eventPool.Unsubscribe(id, handler);
        }

        /// <summary>
        /// 设置默认事件处理函数。
        /// </summary>
        /// <param name="handler">要设置的默认事件处理函数。</param>
        public void SetDefaultHandler(EventHandler<GameEventArgs> handler)
        {
            _eventPool.SetDefaultHandler(handler);
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void Fire(object sender, GameEventArgs e)
        {
            _eventPool.Fire(sender, e);
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void FireNow(object sender, GameEventArgs e)
        {
            _eventPool.FireNow(sender, e);
        }
    }
}