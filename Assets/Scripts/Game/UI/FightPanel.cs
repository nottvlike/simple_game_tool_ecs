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
        actorData.gravity = 10;

        var actorInfo = worldMgr.ActorConfig.Get(actorData.actorId);
        var resourceData = player.GetData<ResourceData>();
        resourceData.resource = actorInfo.resourceName;

        var resourceStateData = player.GetData<ResourceStateData>();
        resourceStateData.name = actorInfo.actorName;
        resourceStateData.isGameObject = true;

        var jumpData = player.GetData<ActorJumpData>();
        jumpData.friction = actorInfo.airFriction;

        var directionData = player.GetData<DirectionData>();
        directionData.x = 1;

        player.SetDirty(resourceData, resourceStateData, jumpData, directionData);
    }

    void OnExitClick()
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.MainPanel);
        uiMgr.HidePanel(PanelType);
    }
}
