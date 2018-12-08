public interface IPoolObject
{
    bool IsInUse { get; set; }
    void Clear();
}

public interface IPool
{
    ObjectData Get(ObjectData data);
    void Release(ObjectData obj);

    T Get<T>() where T : IPoolObject, new();
    void Release<T>(T obj) where T : IPoolObject, new();

    void Destroy();
}
