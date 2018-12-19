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

    public static void ShowYesNoPanel(string content, ButtonClickCallback okCallback, string title = null, string okText = null, 
        string cancelText = null, ButtonClickCallback cancelCallback = null, bool isHideCancel = false)
    {
        if (title == null)
        {
            title = StringUtil.Get("Tips");
        }

        if (okText == null)
        {
            okText = StringUtil.Get("OK");
        }

        if (cancelText == null)
        {
            cancelText = StringUtil.Get("Cancel");
        }

        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.YesNoPanel, title, content, okText, cancelText, okCallback, cancelCallback, isHideCancel);
    }

    public static void ShowYesPanel(string content, ButtonClickCallback okCallback, string title = null, string okText = null)
    {
        ShowYesNoPanel(content, okCallback, title, okText, null, null, true);
    }
}
