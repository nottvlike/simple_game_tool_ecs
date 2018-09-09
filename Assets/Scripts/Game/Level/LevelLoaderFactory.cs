using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    LevelLoader _levelLoader;
    public LevelLoader GetLevelLoader()
    {
        if (_levelLoader == null)
        {
            _levelLoader = new LevelLoader();
            _levelLoader.Load();
        }

        return _levelLoader;
    }

    public void DestroyLevelLoader()
    {
        _levelLoader.Save();
        _levelLoader = null;
    }
}

