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

    protected override void OnShow(params object[] args)
    {
        tips.text = args[0] as string;
    }

    public void OnCloseClick()
    {
        WorldManager.Instance.UIMgr.HidePanel(PanelType);
    }
}
