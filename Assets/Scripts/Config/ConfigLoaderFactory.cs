using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    GameConfig _gameConfig;
    JoyStickConfig _joyStickConfig;

    public GameConfig GameConfig
    {
        get { return _gameConfig; }
    }

    public JoyStickConfig JoyStickConfig
    {
        get { return _joyStickConfig; }
    }

    void LoadConfig()
    {
        _gameConfig = ResourceTool.Instance.Load("GameConfig") as GameConfig;
        _joyStickConfig = ResourceTool.Instance.Load("JoyStickConfig") as JoyStickConfig;
    }
}
