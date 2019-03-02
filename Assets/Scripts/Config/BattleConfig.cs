using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

[System.Serializable]
public struct BattlePreloadResourceInfo
{
    public int preloadId;
    public CreatureType resourceType;
    public CreatureCampType campType;
    public int resourceId;
}

[System.Serializable]
public struct BattleInfo
{
    public int battleId;
    public string battleName;
    public string battleResource;
    public BattlePreloadResourceInfo[] battlePreloadResourceInfoList;
    public BattleVictoryContidion battleVictoryCondition;
}

[System.Serializable]
public struct ChapterInfo
{
    public int chapterId;
    public string chapterName;
    public BattleInfo[] battleInfoList;
}

public class BattleConfig : ScriptableObject
{
    public ChapterInfo[] chapterInfoList;

    ChapterInfo _defaultChapterInfo;
    BattleInfo _defaultBattleInfo;

    public ChapterInfo GetChapterInfo(int chapterId)
    {
        for (var i = 0; i < chapterInfoList.Length; i++)
        {
            var chapterInfo = chapterInfoList[i];
            if (chapterInfo.chapterId == chapterId)
            {
                return chapterInfo;
            }
        }

        LogUtil.E("Failed to find chapterInfo with chapterId {0}!", chapterId);
        return _defaultChapterInfo;
    }

    public BattleInfo GetBattleInfo(int battleId)
    {
        var chapterId = GetChapterId(battleId);
        for (var i = 0; i < chapterInfoList.Length; i++)
        {
            var chapterInfo = chapterInfoList[i];
            if (chapterInfo.chapterId == chapterId)
            {
                var battleInfoList = chapterInfo.battleInfoList;
                for (var j = 0; j < battleInfoList.Length; j++)
                {
                    var battleInfo = battleInfoList[j];
                    if (battleInfo.battleId == battleId)
                    {
                        return battleInfo;
                    }
                }
            }
        }

        LogUtil.E("Failed to find battleInfo with chapterId {0}, battleId {1}!", chapterId, battleId);
        return _defaultBattleInfo;
    }

    public int GetFirstBattleId()
    {
        return chapterInfoList[0].battleInfoList[0].battleId;
    }

    public static int GetChapterId(int battleId)
    {
        return battleId / 1000 * 1000;
    }
}