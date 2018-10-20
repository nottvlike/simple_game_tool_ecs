using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    void RegisterAllModule()
    {
        _moduleList.Add(new Module.JoyStick());
        _moduleList.Add(new Module.Move());
    }
}