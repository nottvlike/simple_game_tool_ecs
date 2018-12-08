using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerItem : MonoBehaviour 
{
    public Button selectButton;
    public Text serverText;

    Data.ServerInfo _serverInfo;

    bool _isSelected;
    public bool IsSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; }
    }

    void Awake()
    {
        selectButton.onClick.AddListener(OnSelectClick);
    }

    void OnSelectClick()
    {
        var panel = WorldManager.Instance.UIMgr.GetPanel(PanelType.RoleSelectPanel) as RoleSelectPanel;
        panel.SelectServer(_serverInfo.serverId);
    }

    public void Init(Data.ServerInfo serverInfo)
    {
        _serverInfo = serverInfo;

        serverText.text = string.Format("{0}", _serverInfo.serverName);
    }
}
