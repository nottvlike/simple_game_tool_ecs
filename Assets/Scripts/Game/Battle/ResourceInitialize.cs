using UnityEngine;
using Data;

public class ResourceInitialize : MonoBehaviour
{
    public int preloadId;

    BattlePreloadResourceInfo _battlePreloadResourceInfo;

    Object _resource;

    public ObjectData ObjData
    {
        set;
        get;
    }

    public bool IsLoaded
    {
        set;
        get;
    }

    public void Init(int battleId)
    {
        var battleInfo = WorldManager.Instance.BattleConfig.GetBattleInfo(battleId);
        var battlePreloadResourceInfo = GetBattlePreloadResourceInfo(battleInfo.Value);
        _battlePreloadResourceInfo = battlePreloadResourceInfo.Value;
        switch (_battlePreloadResourceInfo.resourceType)
        {
            case CreatureType.Actor:
                ObjData = Module.ResourceCreator.CreateActor(_battlePreloadResourceInfo.resourceId, _battlePreloadResourceInfo.campType, transform.position);
                break;
            case CreatureType.BattleItem:
                ObjData = Module.ResourceCreator.CreateBattleItem(_battlePreloadResourceInfo.resourceId, _battlePreloadResourceInfo.campType, transform.position);
                break;
        }
    }

    public void Release()
    {
        if (ObjData != null)
        {
            Module.ResourceCreator.ReleaseResource(ObjData);
            ObjData = null;
            IsLoaded = false;
        }
    }

    BattlePreloadResourceInfo? GetBattlePreloadResourceInfo(BattleInfo battleInfo)
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
        return null;
    }
}
