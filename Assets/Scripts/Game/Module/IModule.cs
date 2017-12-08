public interface IModule
{
    IObject CurrentObject { get; set; }
    void OnAdd(IObject parent);
    void OnRemove(IObject parent);
}
