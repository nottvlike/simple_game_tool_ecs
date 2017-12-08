using System;
using System.Collections.Generic;

public partial class WorldManager : Singleton<WorldManager>
{
    IUITool _uiTool;
    public IUITool GetUITool()
    {
        if (_uiTool == null)
            _uiTool = new UITool();

        return _uiTool;
    }

    public void DestroyUITool()
    {
        _uiTool.Destroy();
        _uiTool = null;
    }
}
