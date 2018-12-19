using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleTipsPanel : Panel
{
    public Button closeButton;
    public Text tips;

    protected override void OnInit()
    {
        closeButton.onClick.AddListener(OnCloseClick);
    }

    public void SetTips(string tipsStr)
    {
        tips.text = tipsStr;
    }

    public void OnCloseClick()
    {
        WorldManager.Instance.UIMgr.HidePanel(PanelType);
    }
}
