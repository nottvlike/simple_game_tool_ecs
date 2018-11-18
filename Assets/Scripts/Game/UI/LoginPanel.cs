using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginNotification : ObjectNotification
{
    LoginPanel _loginPanel;

    public LoginNotification(LoginPanel panel)
    {
        _loginPanel = panel;

        _id = Constant.NOTIFICATION_TYPE_PLAYER;
        _typeList = new int[] { (int)PlayerNotificationType.OnLoginSuccess };
    }

    public override void OnReceive(int type, object notificationData)
    {
        if (_loginPanel != null)
        {
            _loginPanel.OnLoginSuccess((Data.PlayerBaseData)notificationData);
        }
    }
}

public class LoginPanel : MonoBehaviour 
{
    public Button loginButton;
    public InputField nameInput;
    public InputField passwordInput;

    LoginNotification _notification;

    void Awake()
    {
        loginButton.onClick.AddListener(OnLoginClick);

        _notification = new LoginNotification(this);
    }

    void OnLoginClick()
    {
        var worldMgr = WorldManager.Instance;
        var gameServerData = worldMgr.GameServer.GetData<Data.GameServerData>();
        var builder = gameServerData.builder;
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
    }

    void OnEnable()
    {
        _notification.Enabled = true;
    }

    void OnDisable()
    {
        _notification.Enabled = false;
    }
}
