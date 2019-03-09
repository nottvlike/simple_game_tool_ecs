using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class BattleNotification : BaseNotification
{
    BattleInitialize _battleInitialize;

    public BattleNotification(BattleInitialize battleInitialize)
    {
        _battleInitialize = battleInitialize;

        _id = Constant.NOTIFICATION_TYPE_RESOURCE_LOADER;
        _typeList = new int[]{ (int)CreatureStateType.Load, (int)CreatureStateType.Release };
    }

    public override void OnReceive(int type, object notificationData)
    {
        var objData = (ObjectData)notificationData;
        if (type == (int)CreatureStateType.Load)
        {
            _battleInitialize.OnResourceLoaded(objData);
        }
        else
        {
            _battleInitialize.OnResourceReleased(objData);
        }
    }
}

public class BattleInitialize : MonoBehaviour
{
    public int battleId;
    public ResourceInitialize[] resourceInitializeList;

    BattleVictoryContidion _battleVictoryCondition;

    BattleNotification _battleNotification;

    void Awake()
    {
        var battleData = WorldManager.Instance.GameCore.GetData<BattleData>();
        battleData.battleInitialize = this;

        _battleNotification = new BattleNotification(this);

        StartBattle();
    }

    public void StartBattle()
    {
        _battleNotification.Enabled = true;

        var battleInfo = WorldManager.Instance.BattleConfig.GetBattleInfo(battleId);
        _battleVictoryCondition = battleInfo.Value.battleVictoryCondition;

        for (var i = 0; i < resourceInitializeList.Length; i++)
        {
            resourceInitializeList[i].Init(battleId);
        }
    }

    public void StopBattle()
    {
        _battleNotification.Enabled = false;

        for (var i = 0; i < resourceInitializeList.Length; i++)
        {
            var resourceInitialize = resourceInitializeList[i];
            resourceInitialize.Release();
        }
    }

    public void OnResourceLoaded(ObjectData objData)
    {
        for (var i = 0; i < resourceInitializeList.Length; i++)
        {
            var resourceInitialize = resourceInitializeList[i];
            if (resourceInitialize.ObjData != null && resourceInitialize.ObjData == objData)
            {
                resourceInitialize.IsLoaded = true;
            }
        }

        if (CheckAllResourceIsLoaded())
        {
            WorldManager.Instance.UIMgr.HidePanel(PanelType.AsyncPanel);
        }
    }

    bool CheckAllResourceIsLoaded()
    {
        for (var i = 0; i < resourceInitializeList.Length; i++)
        {
            if (!resourceInitializeList[i].IsLoaded)
            {
                return false;
            }
        }

        return true;
    }

    public void OnResourceReleased(ObjectData objData)
    {
        var resourceId = 0;
        for (var i = 0; i < resourceInitializeList.Length; i++)
        {
            var resourceInitialize = resourceInitializeList[i];
            if (resourceInitialize.ObjData != null && resourceInitialize.ObjData == objData)
            {
                resourceId = resourceInitialize.preloadId;

                resourceInitialize.ObjData = null;
                resourceInitialize.IsLoaded = false;
            }
        }

        var needDieActorInfoList = _battleVictoryCondition.needDieActorInfoList;
        for (var i = 0; i < needDieActorInfoList.Length; i++)
        {
            var needDieActorInfo = needDieActorInfoList[i];
            if (needDieActorInfo.preloadId == resourceId)
            {
                needDieActorInfo.isDied = true;
                needDieActorInfoList[i] = needDieActorInfo;
            }
        }

        if (_battleVictoryCondition.limitTime == 0 || CheckAllNeedDieAcotrIsDied())
        {
            StopBattle();

            var uiMgr = WorldManager.Instance.UIMgr;
            uiMgr.HidePanel(PanelType.FightPanel);
            uiMgr.ShowPanel(PanelType.FightResultPanel, true);
        }
    }

    bool CheckAllNeedDieAcotrIsDied()
    {
        var needDieActorInfoList = _battleVictoryCondition.needDieActorInfoList;
        for (var i = 0; i < needDieActorInfoList.Length; i++)
        {
            var needDieActorInfo = needDieActorInfoList[i];
            if (!needDieActorInfo.isDied)
            {
                return false;
            }
        }

        return true;
    }
}
