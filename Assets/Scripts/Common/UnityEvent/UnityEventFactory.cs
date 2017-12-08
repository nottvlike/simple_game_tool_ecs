using UnityEngine;
using System.Collections;

public partial class WorldManager : Singleton<WorldManager>
{
    IUnityEventTool _unityEventTool;
    public IUnityEventTool GetUnityEventTool()
    {
        if (_unityEventTool == null)
            _unityEventTool = UnityEventTool.Instance;

        return _unityEventTool;
    }

    public void DestroyUnityEventTool()
    {
        _unityEventTool.Destroy();
        _unityEventTool = null;
    }
}