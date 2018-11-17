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

        var gameServerData = gameServer.GetData<Data.GameServerData>();
        var builder = gameServerData.builder;
        var name = builder.CreateString("Test123");
        var password = builder.CreateString("123");

        Protocol.Login.ReqLoginGame.StartReqLoginGame(builder);
        Protocol.Login.ReqLoginGame.AddName(builder, name);
        Protocol.Login.ReqLoginGame.AddPassword(builder, password);
        var orc = Protocol.Login.ReqLoginGame.EndReqLoginGame(builder);
        builder.Finish(orc.Value);

        var buf = builder.SizedByteArray();
        SocketMgr.SendMessage(buf, (int)Protocols.ReqLoginGame);

        LogUtil.I("buf " + buf.Length);

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
