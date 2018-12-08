using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRolePanel : Panel 
{
    public Button okButton;
    public Button backButton;

    protected override void OnInit()
    {
        okButton.onClick.AddListener(OnOkClick);
        backButton.onClick.AddListener(OnBackClick);
    }

    void OnOkClick()
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.MainPanel);
        uiMgr.HidePanel(PanelType);
    }

    void OnBackClick()
    {
        WorldManager.Instance.UIMgr.ShowLastShowedPanel();
    }
}
