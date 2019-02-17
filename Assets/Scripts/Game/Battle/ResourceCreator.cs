using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class ResourceCreator : MonoBehaviour
{
    public int preloadId;

    public void Init(int battleId)
    {
        var chapterId = BattleConfig.GetChapterId(battleId);
        var battleInfo = WorldManager.Instance.BattleConfig.GetBattleInfo(chapterId, battleId);
        var battlePreloadResourceInfo = GetBattlePreloadResourceInfo(battleInfo);
        switch (battlePreloadResourceInfo.resourceType)
        {
            case BattleResourceType.Actor:
                LoadActor(battlePreloadResourceInfo.resourceId);
                break;
            case BattleResourceType.BattleItem:
                LoadBattleItem(battlePreloadResourceInfo.resourceId);
                break;
        }
    }

    BattlePreloadResourceInfo _defaultBattlePreloadResourceInfo;
    BattlePreloadResourceInfo GetBattlePreloadResourceInfo(BattleInfo battleInfo)
    {
        var preloadList = battleInfo.battlePreloadResourceInfoList;
        for (var i = 0; i < preloadList.Length; i++)
        {
            var preload = preloadList[i];
            if (preload.preloadId == preloadId)
            {
                return preload;
            }
        }

        LogUtil.E("Failed to find BattlePreloadResourceInfo with preloadId {0}!", preloadId);
        return _defaultBattlePreloadResourceInfo;
    }

    void LoadActor(int actorId)
    {
        var worldMgr = WorldManager.Instance;
        var player = worldMgr.Player;

        var actorInfo = worldMgr.ActorConfig.Get(actorId);

        var physics2DData = player.GetData<Physics2DData>();
        physics2DData.gravity = 10;
        physics2DData.airFriction = actorInfo.airFriction;
        physics2DData.mass = actorInfo.mass;

        var directionData = player.GetData<DirectionData>();
        directionData.direction.x = 1;

        var resourceData = player.GetData<ResourceData>();
        resourceData.initialPosition = transform.position;

        Module.ActorLoader.ReplaceActor(player, actorInfo.actorName, actorInfo.resourceName, physics2DData, directionData);
    }

    void LoadBattleItem(int battleItemId)
    {
        var worldMgr = WorldManager.Instance;
        var battleItemInfo = worldMgr.BattleItemConfig.Get(battleItemId);

        worldMgr.ResourceMgr.LoadAsync(battleItemInfo.resource, delegate (Object obj) {
            var gameObject = Instantiate(obj, transform.position, Quaternion.identity, transform);
        });
    }
}
