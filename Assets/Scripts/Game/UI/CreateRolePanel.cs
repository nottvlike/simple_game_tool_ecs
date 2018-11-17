using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRolePanel : MonoBehaviour 
{
    public Button okButton;
    public Button backButton;

    void Awake()
    {
        okButton.onClick.AddListener(OnOkClick);
        backButton.onClick.AddListener(OnBackClick);
    }

    void OnOkClick()
    {
        WorldManager.Instance.UIMgr.ShowPanel(PanelType.MainPanel);
    }

    void OnBackClick()
    {
        WorldManager.Instance.UIMgr.ShowLastShowedPanel();
    }
}
