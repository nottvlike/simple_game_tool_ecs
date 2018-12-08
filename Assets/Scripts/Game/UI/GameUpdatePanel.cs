using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUpdatePanel : Panel
{
    public Button skipButton;

    protected override void OnInit()
    {
        skipButton.onClick.AddListener(OnSkipClick);
    }

    void OnSkipClick()
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.LoginPanel);
        uiMgr.HidePanel(PanelType);
    }
}
