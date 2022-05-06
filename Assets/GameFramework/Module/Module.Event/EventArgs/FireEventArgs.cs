namespace GameFramework.Module.Event
{
    /// <summary>
    /// 发送事件的基类。
    /// </summary>
    public abstract class FireEventArgs : BaseEventArgs
    {
        /// <summary>
        /// 获取类型编号。
        /// </summary>
        public abstract int Id
        {
            get;
        }
    }
}