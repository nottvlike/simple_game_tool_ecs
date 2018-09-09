using System;
using System.Collections.Generic;

public enum SystemType
{
    None,
}

public struct SystemInfo : IEquatable<SystemInfo>
{
    SystemType systemType;
    PanelType panelType;

    public bool Equals(SystemInfo other)
    {
        return systemType == other.systemType;
    }
}

public partial class WorldManager : Singleton<WorldManager>
{
    Player _player = new Player();
    ItemManager _item = new ItemManager();

    List<ObjectData> _objectDataList = new List<ObjectData>();
    List<BaseObject> _objectList = new List<BaseObject>();
    List<Module> _moduleList = new List<Module>();

    GameConfig _gameConfig;

    public Player Player
    {
        get { return _player; }
    }

    public ItemManager Item
    {
        get { return _item; }
    }

    public List<ObjectData> ObjectDataList
    {
        get { return _objectDataList; }
    }

    public List<BaseObject> ObjectList
    {
        get { return _objectList; }
    }

    public List<Module> ModuleList
    {
        get { return _moduleList; }
    }

    public GameConfig GameConfig
    {
        get { return _gameConfig; }
    }

    public ObjectData GetObjectData(int objId)
    {
        for (var i = 0; i < _objectDataList.Count; i++)
        {
            var objectData = _objectDataList[i];
            if (objId == objectData.ObjectId)
                return objectData;
        }

        return null;
    }

    public BaseObject GetObject(int objId)
    {
        for (var i = 0; i < _objectList.Count; i++)
        {
            var obj = _objectList[i];
            if (objId == obj.Id)
                return obj;
        }

        return null;
    }

    public Module GetModule<T>()
    {
        var moduleType = typeof(T);
        for (var i = 0; i < _moduleList.Count; i++)
        {
            var module = _moduleList[i];
            if (moduleType == module.GetType())
                return module;
        }

        return null;
    }

    public void LaunchGame()
    {
        ResourceTool.Instance.Init("Assets/Resources/");

        RegisterAllModule();

        _gameConfig = ResourceTool.Instance.LoadAssetFile("Prefab/GameConfig") as GameConfig;

        GetLevelLoader().DoDrama();
    }
}
