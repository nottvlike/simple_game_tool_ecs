using System;
using System.Collections.Generic;

public partial class WorldManager : Singleton<WorldManager>
{
    List<ObjectData> _objectDataList = new List<ObjectData>();

    public List<ObjectData> ObjectDataList
    {
        get { return _objectDataList; }
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

    public void LaunchGame()
    {
        ResourceMgr.Init();

        SocketMgr.Init("127.0.0.1", 8888);

        RegisterAllModule();

        var player = Player;
        var item = Item;
        var gameServer = GameServer;
        var gameCore = GameCore;

        LoadConfig();

        UIMgr.ShowPanel(PanelType.GameUpdatePanel);
        LevelLoader.DoDrama();
    }

    public void Destroy()
    {
        DestroySocketMgr();
        DestroyNotificationCenter();
        DestroyPoolMgr();
        DestroyResourceMgr();
        DestroyUIMgr();
        DestroyUnityEventMgr();
        DestroyLevelLoader();
    }
}
