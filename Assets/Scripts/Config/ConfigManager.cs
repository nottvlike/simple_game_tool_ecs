using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    GameConfig _gameConfig;
    JoyStickConfig _joyStickConfig;
    PanelConfig _panelConfig;
    ResourcePreloadConfig _resourcePreloadConfig;

    public GameConfig GameConfig
    {
        get { return _gameConfig; }
    }

    public JoyStickConfig JoyStickConfig
    {
        get { return _joyStickConfig; }
    }

    public PanelConfig PanelConfig
    {
        get { return _panelConfig; }
    }

    public ResourcePreloadConfig ResourcePreloadConfig
    {
        get { return _resourcePreloadConfig; }
    }

    public void LoadConfig()
    {
        _gameConfig = ResourceMgr.Load("GameConfig") as GameConfig;
        _joyStickConfig = ResourceMgr.Load("JoyStickConfig") as JoyStickConfig;
        _panelConfig = ResourceMgr.Load("PanelConfig") as PanelConfig;
    }

    public void LoadPreloadConfig()
    {
        ResourceMgr.LoadAsync("ResourcePreloadConfig", delegate(Object obj)
        {
            _resourcePreloadConfig = obj as ResourcePreloadConfig;

            var resourcePreloadData = GameCore.GetData<Data.ResourcePreloadData>();
            resourcePreloadData.preloadType = ResourcePreloadType.GameInit;
            resourcePreloadData.preloadCount = 0;

            GameCore.SetDirty(resourcePreloadData);
        });
    }
}
