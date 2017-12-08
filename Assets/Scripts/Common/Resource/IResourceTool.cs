using UnityEngine;

public delegate void OnResourceLoadFinished(Object obj);

public interface IResourceTool
{
    void Init(string param);

    bool IsResourceLoaded(string resourceName);
    GameObject Load(string resourceName);
    void LoadAsync(string resourceName, OnResourceLoadFinished func);

    void Destroy();
}
