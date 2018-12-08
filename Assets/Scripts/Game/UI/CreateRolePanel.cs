using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoleNotification : BaseNotification
{
    CreateRolePanel _roleSelectPanel;

    public CreateRoleNotification(CreateRolePanel roleSelectPanel)
    {
        _roleSelectPanel = roleSelectPanel;

        _id = Constant.NOTIFICATION_TYPE_PLAYER;
        _typeList = new int[] { (int)PlayerNotificationType.GetRoleInfoSuccess, (int)PlayerNotificationType.GetRoleInfoFailed };
    }

    public override void OnReceive(int type, ValueType notificationData)
    {
        switch (type)
        {
            case (int)PlayerNotificationType.CreateRoleSuccess:
                _roleSelectPanel.CreateRoleSuccess((Data.RoleInfo)notificationData);
                break;
            case (int)PlayerNotificationType.CreateRoleFailed:
                _roleSelectPanel.CreateRoleFailed((int)notificationData);
                break;
        }
    }
}

public class CreateRolePanel : Panel 
{
    public InputField input;
    public Button okButton;
    public Button backButton;

    CreateRoleNotification _createRoleNotification;

    protected override void OnInit()
    {
        _createRoleNotification = new CreateRoleNotification(this);

        okButton.onClick.AddListener(OnOkClick);
        backButton.onClick.AddListener(OnBackClick);
    }

    protected override void OnShow()
    {
        _createRoleNotification.Enabled = true;
    }

    protected override void OnHide()
    {
        _createRoleNotification.Enabled = false;
    }

    void OnOkClick()
    {
        if (IsNameValid())
        {
            var worldMgr = WorldManager.Instance;
            var socketMgr = worldMgr.SocketMgr;

            var gameServerData = worldMgr.GameServer.GetData<Data.GameNetworkData>();
            var builder = gameServerData.builder;
            builder.Clear();

            var roleName = builder.CreateString(input.text);

            Protocol.Login.ReqCreateRole.StartReqCreateRole(builder);
            Protocol.Login.ReqCreateRole.AddRoleName(builder, roleName);
            var orc = Protocol.Login.ReqCreateRole.EndReqCreateRole(builder);
            builder.Finish(orc.Value);

            var buf = builder.SizedByteArray();
            socketMgr.SendMessage(buf, (int)Protocols.ReqCreateRole);
        }
    }

    bool IsNameValid()
    {
        if (string.IsNullOrEmpty(input.text))
        {
            return false;
        }

        return true;
    }

    void OnBackClick()
    {
        WorldManager.Instance.UIMgr.ShowLastShowedPanel();
    }

    public void CreateRoleSuccess(Data.RoleInfo roleInfo)
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.MainPanel);
        uiMgr.HidePanel(PanelType);
    }

    public void CreateRoleFailed(int result)
    {

    }
}
