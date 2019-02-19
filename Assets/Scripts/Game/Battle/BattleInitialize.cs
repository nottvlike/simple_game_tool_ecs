using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class BattleInitialize : MonoBehaviour
{
    public int battleId;
    public ResourceCreator[] resourceCreatorList;

    BattleVictoryContidion _battleVictoryCondition;

    void Awake()
    {
        var battleData = WorldManager.Instance.GameCore.GetData<BattleData>();
        battleData.battleInitialize = this;

        StartBattle();
    }

    public void StartBattle()
    {
        var battleInfo = WorldManager.Instance.BattleConfig.GetBattleInfo(battleId);
        _battleVictoryCondition = battleInfo.battleVictoryCondition;

        for (var i = 0; i < resourceCreatorList.Length; i++)
        {
            resourceCreatorList[i].Init(battleId);
        }
    }

    public void OnActorDied(int objDataId)
    {
        var resourceId = 0;
        for (var i = 0; i < resourceCreatorList.Length; i++)
        {
            var resourceCreator = resourceCreatorList[i];
            if (resourceCreator.ObjData != null && resourceCreator.ObjData.ObjectId == objDataId)
            {
                resourceId = resourceCreator.preloadId;
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
