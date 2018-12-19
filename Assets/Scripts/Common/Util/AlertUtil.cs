using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertUtil
{
    public static void ShowSimpleTipsPanel(string tips)
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.SimpleTipsPanel, tips);
    }
}
