public interface IPool
{
    ObjectData Get(ObjectData data);
    void Release(ObjectData obj);
    void Destroy();
}
