using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    LevelLoader _levelLoader;
    public LevelLoader LevelLoader
    {
        get
        {
            if (_levelLoader == null)
            {
                _levelLoader = new LevelLoader();
                _levelLoader.Load();
            }

            return _levelLoader;
        }
    }

    void DestroyLevelLoader()
    {
        _levelLoader.Save();
        _levelLoader = null;
    }
}

