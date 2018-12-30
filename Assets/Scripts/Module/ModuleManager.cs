using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class WorldManager : Singleton<WorldManager>
{
    Dictionary<Type, Module.Module> _moduleDict = new Dictionary<Type, Module.Module>();
    List<Module.Module> _moduleList = new List<Module.Module>();

    public List<Module.Module> ModuleList
    {
        get
        {
            return _moduleList;
        }
    }

    public Module.Module GetModule<T>() where T : Module.Module
    {
        Module.Module module;
        if (_moduleDict.TryGetValue(typeof(T), out module))
        {
            return module;
        }

        return null;
    }
    
    void Register(Module.Module module)
    {
        if (_moduleList.IndexOf(module) != -1)
        {
            LogUtil.E("Register uplication module {0}", module.GetType().ToString());
            return;
        }

        _moduleList.Add(module);
        _moduleDict.Add(module.GetType(), module);
    }

    void RegisterAllModule()
    {
        Register(new Module.GameSystem());
        Register(new Module.ActorJoyStick());
        Register(new Module.ActorSyncClient());
        Register(new Module.GameServer());
        Register(new Module.ActorController());
        Register(new Module.ActorMove());
        Register(new Module.ActorJump());
        Register(new Module.ActorFly());
        Register(new Module.ActorDash());
        Register(new Module.ActorStress());
        Register(new Module.ActorPhysics2D());
        Register(new Module.ActorFollowCamera());
        Register(new Module.ActorLoader());
        Register(new Module.ResourcePreloader());
    }
}