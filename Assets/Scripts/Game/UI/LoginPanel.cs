using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct GetAccountInfoResult
{
    public int result;
    public string accountId;
    public List<Data.RoleInfoLite> roleInfoLiteList;
}

public class LoginPanel : Panel
{
    public Button loginButton;
    public InputField nameInput;
    public InputField passwordInput;

    protected override void OnInit()
    {
        loginButton.onClick.AddListener(OnLoginClick);
    }

    void OnLoginClick()
    {
        var worldMgr = WorldManager.Instance;
        var gameServerData = worldMgr.GameServer.GetData<Data.GameNetworkData>();
        var builder = gameServerData.builder;
        builder.Clear();

        var userName = builder.CreateString(nameInput.text);
        var password = builder.CreateString(passwordInput.text);

        var loginUrl = string.Format("http://127.0.0.1:8001/login?user={0}&password={1}", userName, password);
        HttpUtil.GetAsync(loginUrl, delegate (WebRequestResultType resultType, string accountInfoStr)
        {
            if (resultType == WebRequestResultType.Success)
            {
                var accountInfoResult = JsonUtility.FromJson<GetAccountInfoResult>(accountInfoStr);
                var playerBaseData = WorldManager.Instance.Player.GetData<Data.PlayerBaseData>();
                var result = accountInfoResult.result;
                if (accountInfoResult.result == 0)
                {
                    playerBaseData.accountId = accountInfoResult.accountId;
                    playerBaseData.roleInfoLiteList.Clear();
                    playerBaseData.roleInfoLiteList.AddRange(accountInfoResult.roleInfoLiteList);

                    LoginSuccess(playerBaseData);
                }
                else
                {
                    LoginFailed(accountInfoResult.result);
                }
            }
        });
    }

    public void LoginSuccess(Data.PlayerBaseData data)
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

    public void LoginFailed(int result)
    {

    }
}
