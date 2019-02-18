using UnityEngine;
using Data;

public class ResourceCreator : MonoBehaviour
{
    public int preloadId;

    BattlePreloadResourceInfo _battlePreloadResourceInfo;

    Object _resource;
    ObjectData _objData;
    public ObjectData Actor
    {
        get { return _objData; }
    }

    public void Init(int battleId)
    {
        var battleInfo = WorldManager.Instance.BattleConfig.GetBattleInfo(battleId);
        _battlePreloadResourceInfo = GetBattlePreloadResourceInfo(battleInfo);
        switch (_battlePreloadResourceInfo.resourceType)
        {
            case BattleResourceType.Player:
                _objData = LoadActor(_battlePreloadResourceInfo.resourceId, ActorCampType.Player);
                break;
            case BattleResourceType.Enemy:
                _objData = LoadActor(_battlePreloadResourceInfo.resourceId, ActorCampType.Enemy);
                break;
            case BattleResourceType.BattleItem:
                LoadBattleItem(_battlePreloadResourceInfo.resourceId);
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

    ObjectData LoadActor(int actorId, ActorCampType camp)
    {
        var worldMgr = WorldManager.Instance;
        ObjectData actor = null;
        if (camp == ActorCampType.Enemy)
        {
            actor = worldMgr.PoolMgr.GetObjData(worldMgr.Player);
            actor.RemoveData<ClientJoyStickData>();
            actor.RemoveData<FollowCameraData>();
        }
        else
        {
            actor = worldMgr.Player;
        }

        var actorData = actor.GetData<ActorData>();
        actorData.actorId = actorId;
        actorData.camp = camp;

        var actorInfo = worldMgr.ActorConfig.Get(actorId);

        var physics2DData = actor.GetData<Physics2DData>();
        physics2DData.gravity = 10;
        physics2DData.airFriction = actorInfo.airFriction;
        physics2DData.mass = actorInfo.mass;

        var directionData = actor.GetData<DirectionData>();
        directionData.direction.x = 1;

        var resourceData = actor.GetData<ResourceData>();
        resourceData.initialPosition = transform.position;

        Module.ActorLoader.ReplaceActor(actor, actorInfo.actorName, actorInfo.resourceName, physics2DData, directionData);

        return actor;
    }

    void LoadBattleItem(int battleItemId)
    {
        if (_resource != null)
        {
            var worldMgr = WorldManager.Instance;
            var battleItemInfo = worldMgr.BattleItemConfig.Get(battleItemId);

            worldMgr.ResourceMgr.LoadAsync(battleItemInfo.resource, delegate (UnityEngine.Object obj)
            {
                _resource = Instantiate(obj, transform.position, Quaternion.identity, transform);
            });
        }
    }
}
