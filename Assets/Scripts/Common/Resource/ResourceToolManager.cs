using System;
using System.Collections.Generic;

public partial class WorldManager : Singleton<WorldManager>
{
    IResourceTool _resourceMgr;
    public IResourceTool ResourceMgr
    {
        get
        {
            if (_resourceMgr == null)
                _resourceMgr = ResourceTool.Instance;

            return _resourceMgr;
        }
    }

    void DestroyResourceMgr()
    {
        _resourceMgr.Destroy();
        _resourceMgr = null;
    }
}
