using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    GameConfig _gameConfig;
    JoyStickConfig _joyStickConfig;
    PanelConfig _panelConfig;

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

    void LoadConfig()
    {
        _gameConfig = ResourceTool.Instance.Load("GameConfig") as GameConfig;
        _joyStickConfig = ResourceTool.Instance.Load("JoyStickConfig") as JoyStickConfig;
        _panelConfig = ResourceTool.Instance.Load("PanelConfig") as PanelConfig;
    }
}
