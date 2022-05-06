namespace GameFramework.Module
{
    public interface IEasyPool<T> where T : class
    {
        int CountInactive { get; }

        T Get();

        void Release(T element);

        void Clear();
    }
}


