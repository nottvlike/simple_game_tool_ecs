using UnityEngine;
using System.Collections;

public partial class WorldManager : Singleton<WorldManager>
{
    IUnityEventTool _unityEventMgr;
    public IUnityEventTool UnityEventMgr
    {
        get
        {
            if (_unityEventMgr == null)
                _unityEventMgr = UnityEventTool.Instance;

            return _unityEventMgr;
        }
    }

    void DestroyUnityEventMgr()
    {
        if (_unityEventMgr != null)
        {
            _unityEventMgr.Destroy();
            _unityEventMgr = null;
        }
    }
}