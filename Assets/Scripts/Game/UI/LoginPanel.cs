using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginNotification : BaseNotification
{
    LoginPanel _loginPanel;

    public LoginNotification(LoginPanel panel)
    {
        _loginPanel = panel;

        _id = Constant.NOTIFICATION_TYPE_PLAYER;
        _typeList = new int[] { (int)PlayerNotificationType.OnLoginSuccess, (int)PlayerNotificationType.OnLoginFailed };
    }

    public override void OnReceive(int type, object notificationData)
    {
        switch (type)
        {
            case (int)PlayerNotificationType.OnLoginSuccess:
                _loginPanel.OnLoginSuccess((Data.PlayerBaseData)notificationData);
                break;
        }
    }

    public override void OnReceive(int type, ValueType notificationData)
    {
        switch (type)
        {
            case (int)PlayerNotificationType.OnLoginFailed:
                _loginPanel.OnLoginFailed((int)notificationData);
                break;
        }
    }
}

public class LoginPanel : Panel 
{
    public Button loginButton;
    public InputField nameInput;
    public InputField passwordInput;

    LoginNotification _notification;

    protected override void OnInit()
    {
        loginButton.onClick.AddListener(OnLoginClick);

        _notification = new LoginNotification(this);
    }

    protected override void OnShow()
    {
        _notification.Enabled = true;
    }

    protected override void OnHide()
    {
        _notification.Enabled = false;
    }

    void OnLoginClick()
    {
        var worldMgr = WorldManager.Instance;
        var gameServerData = worldMgr.GameServer.GetData<Data.GameNetworkData>();
        var builder = gameServerData.builder;
        builder.Clear();

        var userName = builder.CreateString(nameInput.text);
        var password = builder.CreateString(passwordInput.text);

        Protocol.Login.ReqLoginGame.StartReqLoginGame(builder);
        Protocol.Login.ReqLoginGame.AddName(builder, userName);
        Protocol.Login.ReqLoginGame.AddPassword(builder, password);
        var orc = Protocol.Login.ReqLoginGame.EndReqLoginGame(builder);
        builder.Finish(orc.Value);

        var buf = builder.SizedByteArray();
        worldMgr.SocketMgr.SendMessage(buf, (int)Protocols.ReqLoginGame);
    }

    public void OnLoginSuccess(Data.PlayerBaseData data)
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        if (data.roleInfoLiteList.Count > 0)
        {
            uiMgr.ShowPanel(PanelType.RoleSelectPanel);
        }
        else
        {
            uiMgr.ShowPanel(PanelType.CreateRolePanel);
        }

        uiMgr.HidePanel(PanelType);
    }

    public void OnLoginFailed(int result)
    {

    }
}
