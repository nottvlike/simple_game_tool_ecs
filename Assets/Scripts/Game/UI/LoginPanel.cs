using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LoginGameResult
{
    Success = 0,
    NoneAccount,
    WrongPassword,
    Unkonw
}

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

        var userName = nameInput.text;
        var password = passwordInput.text;

        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.AsyncPanel);

        var loginUrl = string.Format("http://127.0.0.1:8001/login?user={0}&password={1}", userName, password);
        HttpUtil.GetAsync(loginUrl, delegate (WebRequestResultType resultType, string accountInfoStr)
        {
            uiMgr.HidePanel(PanelType.AsyncPanel);

            if (resultType == WebRequestResultType.Success)
            {
                var accountInfoResult = JsonUtility.FromJson<GetAccountInfoResult>(accountInfoStr);
                var playerBaseData = WorldManager.Instance.GameCore.GetData<Data.PlayerBaseData>();
                var result = (LoginGameResult)accountInfoResult.result;
                if (result == LoginGameResult.Success)
                {
                    playerBaseData.accountId = accountInfoResult.accountId;
                    playerBaseData.roleInfoLiteList.Clear();
                    playerBaseData.roleInfoLiteList.AddRange(accountInfoResult.roleInfoLiteList);

                    LoginSuccess(playerBaseData);
                }
                else
                {
                    LoginFailed(result);
                }
            }
            else
            {
                LoginFailed(LoginGameResult.Unkonw);
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

    public void LoginFailed(LoginGameResult result)
    {
        string tips = "";
        switch(result)
        {
            case LoginGameResult.NoneAccount:
                tips = StringUtil.Get("Login failed, account not found!");
                break;
            case LoginGameResult.WrongPassword:
                tips = StringUtil.Get("Login failed, wrong password!");
                break;
            case LoginGameResult.Unkonw:
                tips = StringUtil.Get("Login failed, server error!");
                break;
            default:
                LogUtil.W("Unknow LoginGameResult type {0}", result);
                break;
        }

        AlertUtil.ShowSimpleTipsPanel(tips);
    }
}
