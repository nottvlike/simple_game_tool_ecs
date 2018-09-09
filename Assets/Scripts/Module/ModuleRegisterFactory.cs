using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    public void RegisterAllModule()
    {
        _moduleList.Add(new MoveModule());
    }
}