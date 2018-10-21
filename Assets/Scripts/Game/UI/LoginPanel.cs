using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour 
{
    public Button loginButton;
    public Button serverButton;
    public Button exitButton;

    void Awake()
    {
        loginButton.onClick.AddListener(OnLoginClick);
        serverButton.onClick.AddListener(OnShowServerPanelClick);
        exitButton.onClick.AddListener(OnExitClick);
    }

    void OnLoginClick()
    {
        WorldManager.Instance.GetUITool().ShowPanel(PanelType.CreateRolePanel);
    }

    void OnShowServerPanelClick()
    {
        WorldManager.Instance.GetUITool().ShowPanel(PanelType.ServerPanel);
    }

    void OnExitClick()
    {

    }
}
