public abstract class PoolObject
{
    public bool IsInUse
    {
        set;
        get;
    }

    public abstract void Clear();
}

public interface IPool
{
    ObjectData Get(ObjectData data);
    void Release(ObjectData obj);

    T Get<T>() where T : PoolObject, new();
    void Release<T>(T obj) where T : PoolObject, new();

    void Destroy();
}
