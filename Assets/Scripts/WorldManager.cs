using System;
using System.Collections.Generic;
using UnityEngine;

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

        TestHttpUtil();

        RegisterAllModule();

        var player = Player;
        var item = Item;
        var gameServer = GameServer;
        var gameCore = GameCore;

        LoadConfig();

        UIMgr.ShowPanel(PanelType.GameUpdatePanel);
        LevelLoader.DoDrama();
    }

    #region Test

    void TestHttpUtil()
    {
        var result = HttpUtil.Get("http://www.baidu.com");
        LogUtil.I(result);

        HttpUtil.GetAsync("http://www.baidu.com", delegate (WebRequestResultType resultType, string result1)
        {
            LogUtil.I(result1);
        });
    }

    #endregion

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
