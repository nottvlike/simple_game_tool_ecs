using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerPanel : Panel 
{
    public Button closeButton;

    protected override void OnInit()
    {
        closeButton.onClick.AddListener(OnCloseClick);
    }

    void OnCloseClick()
    {
        WorldManager.Instance.UIMgr.HidePanel(PanelType);
    }
}
