using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRoleInfoNotification : BaseNotification
{
    RoleSelectPanel _roleSelectPanel;

    public GetRoleInfoNotification(RoleSelectPanel roleSelectPanel)
    {
        _roleSelectPanel = roleSelectPanel;

        _id = Constant.NOTIFICATION_TYPE_PLAYER;
        _typeList = new int[] { (int)PlayerNotificationType.GetRoleInfoSuccess, (int)PlayerNotificationType.GetRoleInfoFailed };
    }

    public override void OnReceive(int type, ValueType notificationData)
    {
        switch (type)
        {
            case (int)PlayerNotificationType.GetRoleInfoSuccess:
                _roleSelectPanel.GetRoleInfoSuccess((Data.RoleInfo)notificationData);
                break;
            case (int)PlayerNotificationType.GetRoleInfoFailed:
                _roleSelectPanel.GetRoleInfoFailed((int)notificationData);
                break;
        }
    }
}

public class RoleSelectPanel : Panel 
{
    public Button showServerPanelButton;
    public Text currentServer;

    public Button goButton;

    public GridLayoutGroup grid;
    public RoleSelectItem roleSelectItemTemplate;

    List<RoleSelectItem> _roleSelectItemList = new List<RoleSelectItem>();

    Data.ServerInfo _defaultServerInfo = new Data.ServerInfo();

    Data.RoleInfoLite _selectedRole;
    int _selectedServer;

    GetRoleInfoNotification getRoleInfoNotification;

    protected override void OnInit() 
    {
        getRoleInfoNotification = new GetRoleInfoNotification(this);

        showServerPanelButton.onClick.AddListener(OnShowServerPanelClick);
        goButton.onClick.AddListener(OnGoClick);
    }

    protected override void OnShow(params object[] args)
    {
        var player = WorldManager.Instance.Player;
        var playerBaseData = player.GetData<Data.PlayerBaseData>();
        var roleInfoLiteList = playerBaseData.roleInfoLiteList;
        for (var i = 0; i < roleInfoLiteList.Count; i++)
        {
            RoleSelectItem item;
            if (i < _roleSelectItemList.Count)
            {
                item = _roleSelectItemList[i];
            }
            else
            {
                item = Instantiate(roleSelectItemTemplate);
                item.transform.localScale = roleSelectItemTemplate.transform.localScale;
                item.transform.localRotation = roleSelectItemTemplate.transform.localRotation;
                item.transform.SetParent(grid.transform);

                _roleSelectItemList.Add(item);
            }

            item.Init(this, roleInfoLiteList[i]);
            item.gameObject.SetActive(true);
        }

        for (var i = roleInfoLiteList.Count; i < _roleSelectItemList.Count; i++)
        {
            _roleSelectItemList[i].gameObject.SetActive(false);
        }

        var serverData = player.GetData<Data.ServerData>();
        var serverInfoList = serverData.serverInfoList;
        if (serverInfoList.Count > 0)
        {
            SelectServer(serverInfoList[0].serverId);
        }

        if (roleInfoLiteList.Count > 0)
        {
            SelectServer(roleInfoLiteList[0].serverId);
        }

        getRoleInfoNotification.Enabled = true;
    }

    protected override void OnHide()
    {
        getRoleInfoNotification.Enabled = false;
    }

    void OnShowServerPanelClick()
    {
        WorldManager.Instance.UIMgr.ShowPanel(PanelType.ServerPanel);
    }

    void OnGoClick()
    {
        var worldMgr = WorldManager.Instance;
        var socketMgr = worldMgr.SocketMgr;
        var serverInfo = GetServerInfo(_selectedServer);
        socketMgr.Init(serverInfo.serverAddress, serverInfo.serverPort);

        if (HasRole(_selectedServer))
        {
            var gameServerData = worldMgr.GameServer.GetData<Data.GameNetworkData>();
            var builder = gameServerData.builder;
            builder.Clear();

            var roleId = builder.CreateString(_selectedRole.roleId);

            Protocol.Login.ReqRoleInfo.StartReqRoleInfo(builder);
            Protocol.Login.ReqRoleInfo.AddRoleId(builder, roleId);
            var orc = Protocol.Login.ReqRoleInfo.EndReqRoleInfo(builder);
            builder.Finish(orc.Value);

            var buf = builder.SizedByteArray();
            socketMgr.SendMessage(buf, (int)Protocols.ReqRoleInfo);
        }
        else
        {
            var uiMgr = worldMgr.UIMgr;
            uiMgr.ShowPanel(PanelType.CreateRolePanel);
            uiMgr.HidePanel(PanelType);
        }
    }

    public void GetRoleInfoSuccess(Data.RoleInfo roleInfo)
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.MainPanel);
        uiMgr.HidePanel(PanelType);
    }

    public void GetRoleInfoFailed(int result)
    {

    }

    bool HasRole(int serverId)
    {
        var playerBaseData = WorldManager.Instance.Player.GetData<Data.PlayerBaseData>();
        var roleInfoLiteList = playerBaseData.roleInfoLiteList;
        for (var i = 0; i < roleInfoLiteList.Count; i++)
        {
            var roleInfoLite = roleInfoLiteList[i];
            if (roleInfoLite.serverId == serverId)
            {
                return true;
            }
        }

        return false;
    }

    public Data.ServerInfo GetServerInfo(int serverId)
    {
        var serverData = WorldManager.Instance.Player.GetData<Data.ServerData>();
        var serverInfoList = serverData.serverInfoList;
        for (var i = 0; i < serverInfoList.Count; i++)
        {
            var serverInfo = serverInfoList[i];
            if (serverInfo.serverId == serverId)
            {
                return serverInfo;
            }
        }

        LogUtil.E("Failed to find server info {0}", serverId);
        return _defaultServerInfo;
    }

    public void SelectRole(Data.RoleInfoLite roleInfoLite)
    {
        _selectedRole = roleInfoLite;

        SelectServer(roleInfoLite.serverId);
    }

    public void SelectServer(int serverId)
    {
        if (_selectedServer == serverId)
        {
            return;
        }

        _selectedServer = serverId;

        var serverData = WorldManager.Instance.Player.GetData<Data.ServerData>();
        serverData.serverInfo = GetServerInfo(_selectedServer);
        currentServer.text = serverData.serverInfo.serverName;
    }
}
