using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleSelectPanel : MonoBehaviour 
{
    public Button showServerPanelButton;
    public Text currentServer;
    public Button goButton;
    public GridLayoutGroup grid;
    public RoleSelectItem roleSelectItemTemplate;

	void Awake() 
    {
        showServerPanelButton.onClick.AddListener(OnShowServerPanelClick);
        goButton.onClick.AddListener(OnGoClick);
    }

    void OnShowServerPanelClick()
    {
        WorldManager.Instance.UIMgr.ShowPanel(PanelType.ServerPanel);
    }

    void OnGoClick()
    {
        var currentServerId = 0;
        if (HasRole(currentServerId))
        {
            WorldManager.Instance.UIMgr.ShowPanel(PanelType.MainPanel);
        }
        else
        {
            WorldManager.Instance.UIMgr.ShowPanel(PanelType.CreateRolePanel);
        }
    }

    bool HasRole(int serverId)
    {
        return false;
    }
}
