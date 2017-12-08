public interface IUnityEventTool
{
    void Add(IUpdateEvent update);
    void Remove(IUpdateEvent update);

    void Destroy();
}
