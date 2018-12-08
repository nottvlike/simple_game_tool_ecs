using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel : Panel
{
    public Button fight;

    protected override void OnInit()
    {
        fight.onClick.AddListener(OnFightClick);
    }

    void OnFightClick()
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.FightPanel);
        uiMgr.HidePanel(PanelType);
    }
}
