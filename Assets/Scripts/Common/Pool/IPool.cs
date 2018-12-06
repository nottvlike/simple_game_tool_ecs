public interface IPoolObject
{
    bool GetIsInUse();
    void SetIsInUse(bool inUse);
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
