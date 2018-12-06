using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleSelectItem : MonoBehaviour 
{
    public Button selectButton;
    public Text roleInfoText;

    RoleSelectPanel _parent;

	void Awake() 
    {
        selectButton.onClick.AddListener(OnSelectClick);	
	}
	
	void OnSelectClick() 
    {
		
	}

    public void Init(RoleSelectPanel parent, Data.RoleInfoLite roleInfoLite)
    {
        _parent = parent;
    }
}
