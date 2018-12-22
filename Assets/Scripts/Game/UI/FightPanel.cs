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
        var resourceData = player.GetData<ResourceData>();
        resourceData.resource = actorInfo.resourceName;

        var resourceStateData = player.GetData<ResourceStateData>();
        resourceStateData.name = actorInfo.actorName;
        resourceStateData.isGameObject = true;

        player.SetDirty(resourceData, resourceStateData);
    }

    void OnExitClick()
    {
        var uiMgr = WorldManager.Instance.UIMgr;
        uiMgr.ShowPanel(PanelType.MainPanel);
        uiMgr.HidePanel(PanelType);
    }
}
