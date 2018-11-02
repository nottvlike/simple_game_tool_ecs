using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    void RegisterAllModule()
    {
        _moduleList.Add(new Module.GameSystem());
#if UNITY_EDITOR
        _moduleList.Add(new Module.ActorJoyStick());
#endif
        _moduleList.Add(new Module.ActorSyncClient());
        _moduleList.Add(new Module.GameServer());
        _moduleList.Add(new Module.ActorController());
        _moduleList.Add(new Module.ActorMove());
        _moduleList.Add(new Module.ResourceLoader());
    }
}