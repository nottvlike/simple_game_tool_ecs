using UnityEngine;
using Data;

public class ResourceCreator : MonoBehaviour
{
    public int preloadId;

    BattlePreloadResourceInfo _battlePreloadResourceInfo;

    Object _resource;
    ObjectData _objData;
    public ObjectData ObjData
    {
        get { return _objData; }
    }

    public void Init(int battleId)
    {
        var battleInfo = WorldManager.Instance.BattleConfig.GetBattleInfo(battleId);
        _battlePreloadResourceInfo = GetBattlePreloadResourceInfo(battleInfo);
        switch (_battlePreloadResourceInfo.resourceType)
        {
            case ResourceType.Actor:
                _objData = Module.ResourceLoader.CreateActor(_battlePreloadResourceInfo.resourceId, _battlePreloadResourceInfo.campType, transform.position);
                break;
            case ResourceType.BattleItem:
                _objData = Module.ResourceLoader.CreateBattleItem(_battlePreloadResourceInfo.resourceId, _battlePreloadResourceInfo.campType, transform.position);
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
}
