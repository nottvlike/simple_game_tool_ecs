using System;
using System.Collections.Generic;

public partial class WorldManager : Singleton<WorldManager>
{
    IUITool _uiMgr;
    public IUITool UIMgr
    {
        get
        {
            if (_uiMgr == null)
            {
                _uiMgr = new UITool();
                _uiMgr.Init();
            }

            return _uiMgr;
        }
    }

    void DestroyUIMgr()
    {
        _uiMgr.Destroy();
        _uiMgr = null;
    }
}
