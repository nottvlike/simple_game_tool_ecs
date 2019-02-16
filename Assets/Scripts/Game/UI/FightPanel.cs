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
        var player = worldMgr.Player;

        var actorData = player.GetData<ActorData>();
        var actorInfo = worldMgr.ActorConfig.Get(actorData.actorId);

        var physics2DData = player.GetData<Physics2DData>();
        physics2DData.gravity = 10;
        physics2DData.airFriction = actorInfo.airFriction;
        physics2DData.mass = actorInfo.mass;

        var directionData = player.GetData<DirectionData>();
        directionData.direction.x = 1;

        player.SetDirty(physics2DData, directionData);

        Module.ActorLoader.ReplaceActor(player, actorInfo.actorName, actorInfo.resourceName);
    }

    void OnExitClick()
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.MainPanel);
        uiMgr.HidePanel(PanelType);
    }
}
