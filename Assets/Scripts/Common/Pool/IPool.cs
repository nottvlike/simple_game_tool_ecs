public interface IPool
{
    T Get<T>() where T : IPoolObject, new();
    void Release(IPoolObject obj);
    void Destroy();
}
