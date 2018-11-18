using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleSelectItem : MonoBehaviour 
{
    public Button selectButton;
    public Text roleInfoText;

	void Awake() 
    {
        selectButton.onClick.AddListener(OnSelectClick);	
	}
	
	void OnSelectClick() 
    {
		
	}
}
