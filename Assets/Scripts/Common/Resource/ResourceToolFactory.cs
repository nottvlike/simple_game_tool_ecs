using System;
using System.Collections.Generic;

public partial class WorldManager : Singleton<WorldManager>
{
    IResourceTool _resourceTool;
    public IResourceTool GetResourceTool()
    {
        if (_resourceTool == null)
            _resourceTool = ResourceTool.Instance;

        return _resourceTool;
    }

    public void DestroyResourceTool()
    {
        _resourceTool.Destroy();
        _resourceTool = null;
    }
}
