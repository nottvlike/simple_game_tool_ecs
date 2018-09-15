using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    GameConfig _gameConfig;

    void LoadConfig()
    {
        _gameConfig = ResourceTool.Instance.Load("GameConfig") as GameConfig;
    }
}
