using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class FightPanel : Panel 
{
    public Button exit;

    protected override void OnInit()
    {
        exit.onClick.AddListener(OnExitClick);
    }

    protected override void OnShow(params object[] args)
    {
        var worldMgr = WorldManager.Instance;
        var battleConfig = worldMgr.BattleConfig;

        var battleId = battleConfig.GetFirstBattleId();
        var battleInfo = battleConfig.GetBattleInfo(battleId);

        var battleData = worldMgr.GameCore.GetData<BattleData>();
        if (battleData.battleInitialize == null)
        {
            worldMgr.ResourceMgr.LoadAsync(battleInfo.Value.battleResource, delegate (Object obj) {
                Instantiate(obj);
            });
        }

        worldMgr.UIMgr.ShowPanel(PanelType.AsyncPanel);
    }

    void OnExitClick()
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.MainPanel);
        uiMgr.HidePanel(PanelType);
    }
}
