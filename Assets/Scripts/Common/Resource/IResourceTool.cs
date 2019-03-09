using UnityEngine;

public delegate void OnResourceLoadFinished(Object obj);

public interface IResourceTool
{
    void Init();

    bool IsResourceLoaded(string resourceName);
    Object Load(string resourceName);
    void LoadAsync(string resourceName, OnResourceLoadFinished func);

    ResourceInfo? Get(string resourceName);

    void Destroy();
}
