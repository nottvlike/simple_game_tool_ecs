public interface IUnityEventTool
{
    void Add(IUpdateEvent update);
    void Remove(IUpdateEvent update);

    void Add(IFixedUpdateEvent update);
    void Remove(IFixedUpdateEvent update);

    void Add(ILateUpdateEvent update);
    void Remove(ILateUpdateEvent update);

    void Destroy();
}
