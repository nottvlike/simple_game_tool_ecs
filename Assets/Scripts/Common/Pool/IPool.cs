public interface IPool
{
    T Get<T>() where T : PoolObject, new();
    void Release(PoolObject obj);
    void Destroy();
}
