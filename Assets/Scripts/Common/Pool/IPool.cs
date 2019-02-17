using System;

public interface IPoolObject
{
    bool IsInUse { get; set; }
    void Clear();
}

public interface IPool
{
    ObjectData GetObjData();
    ObjectData GetObjData(ObjectData data);
    void ReleaseObjData(ObjectData obj);

    Data.Data GetData(Type type);
    void ReleaseData(Data.Data obj);

    T Get<T>() where T : IPoolObject;
    void Release(IPoolObject obj);

    void Destroy();
}
