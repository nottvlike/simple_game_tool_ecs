using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BattleItemInfo
{
    public int id;
    public string resource;
    public string itemName;
    public int attackId;
}

public class BattleItemConfig : ScriptableObject
{
    public BattleItemInfo[] battleItemInfoList;

    BattleItemInfo _defaultBattleItemInfo;
    public BattleItemInfo Get(int battleItemId)
    {
        for (var i = 0; i < battleItemInfoList.Length; i++)
        {
            var battleItemInfo = battleItemInfoList[i];
            if (battleItemInfo.id == battleItemId)
            {
                return battleItemInfo;
            }
        }

        LogUtil.E("Failed to find BattleItemInfo " + battleItemId);
        return _defaultBattleItemInfo;
    }
}
