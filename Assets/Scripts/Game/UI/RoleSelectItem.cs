using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleSelectItem : MonoBehaviour 
{
    public Button selectButton;
    public Text roleInfoText;

    RoleSelectPanel _parent;
    Data.RoleInfoLite _roleInfoLite;

    void Awake() 
    {
        selectButton.onClick.AddListener(OnSelectClick);	
	}
	
	void OnSelectClick() 
    {
        _parent.SelectRole(_roleInfoLite);
	}

    public void Init(RoleSelectPanel parent, Data.RoleInfoLite roleInfoLite)
    {
        _parent = parent;
        _roleInfoLite = roleInfoLite;

        var serverInfo = parent.GetServerInfo(_roleInfoLite.serverId);
        roleInfoText.text = string.Format("{0} {1}({3})", _roleInfoLite.roleName, _roleInfoLite.roleLevel, _roleInfoLite, serverInfo.serverName);
    }
}
