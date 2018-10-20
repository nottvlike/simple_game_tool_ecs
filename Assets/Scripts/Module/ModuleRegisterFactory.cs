using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    void RegisterAllModule()
    {
        _moduleList.Add(new Module.GameSystem());
        _moduleList.Add(new Module.ActorJoyStick());
        _moduleList.Add(new Module.ActorSyncClient());
        _moduleList.Add(new Module.GameServer());
        _moduleList.Add(new Module.ActorController());
        _moduleList.Add(new Module.ActorMove());
    }
}