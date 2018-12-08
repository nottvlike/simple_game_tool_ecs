using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightPanel : Panel 
{
    public Button left;
    public Button right;
    public Button jump;
    public Button skillDefault;
    public Button skillCustom;

    public Button exit;

    protected override void OnInit()
    {
        left.onClick.AddListener(OnLeftClick);
        right.onClick.AddListener(OnRightClick);
        jump.onClick.AddListener(OnJumpClick);
        skillDefault.onClick.AddListener(OnSkillDefaultClick);
        skillCustom.onClick.AddListener(OnSkillCustomClick);

        exit.onClick.AddListener(OnExitClick);
    }

    void OnLeftClick()
    {

    }

    void OnRightClick()
    {

    }

    void OnJumpClick()
    {

    }

    void OnSkillDefaultClick()
    {

    }

    void OnSkillCustomClick()
    {

    }

    void OnExitClick()
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.MainPanel);
        uiMgr.HidePanel(PanelType);
    }
}
