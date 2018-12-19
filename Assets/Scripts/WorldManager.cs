using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePreloadNotification : BaseNotification
{
    public ResourcePreloadNotification()
    {
        _id = Constant.NOTIFICATION_TYPE_RESOURCE;
        _typeList = new int[] { (int)ResourceNotificationType.InitFinished };

        Enabled = true;
    }

    public override void OnReceive(int type, ValueType notificationData)
    {
        var worldMgr = WorldManager.Instance;
        var player = worldMgr.Player;
        var item = worldMgr.Item;
        var gameServer = worldMgr.GameServer;
        var gameCore = worldMgr.GameCore;

        worldMgr.LoadConfig();

        HttpUtil.GetAsync("http://127.0.0.1:8001/serverInfo", delegate (WebRequestResultType resultType, string serverInfoStr)
        {
            if (resultType == WebRequestResultType.Success)
            {
                var serverInfoResult = JsonUtility.FromJson<GetServerInfoResult>(serverInfoStr);
                var serverData = player.GetData<Data.ServerData>();
                var result = serverInfoResult.result;
                if (serverInfoResult.result == 0)
                {
                    serverData.serverInfoList.Clear();
                    serverData.serverInfoList.AddRange(serverInfoResult.serverInfoList);
                }
            }
        });

        worldMgr.UIMgr.ShowPanel(PanelType.GameUpdatePanel);
        worldMgr.LevelLoader.DoDrama();
    }
}

public partial class WorldManager : Singleton<WorldManager>
{
    List<ObjectData> _objectDataList = new List<ObjectData>();

    ResourcePreloadNotification _resourcePreloadNotification;

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
        _resourcePreloadNotification = new ResourcePreloadNotification();

        ResourceMgr.Init();

        RegisterAllModule();
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
