using System;
using System.Collections.Generic;
using UnityEngine;
using Data;

[Serializable]
public class PlayerInfo
{
    public PositionData positionData;
    public DirectionData directionData;
    public SpeedData speedData;
    public ResourceData resourceData;
    public ResourceStateData resourceStateData;
}

[Serializable]
public class TalkingInfo
{
    public int id;
    public string name;
    public string content;
    public string resource;
    public bool last;
}

public enum DramaItemType
{
    CreatePlayer,
    CreateEnemy,
    Talking,
    Video
}

[Serializable]
public class DramaInfo
{
    public DramaItemType dramaItemType;
    public int id;
}

[Serializable]
public class LevelInfo
{
    public int levelId;
    public string levelName;

    public List<DramaInfo> dramaItemList;
}

[Serializable]
public class LevelConfig
{
    public PlayerInfo playerData;
    public TalkingInfo[] talkingData;

    public LevelInfo levelData;
}

public class GameConfig : ScriptableObject
{
    public LevelConfig[] levelConfigList;

    public LevelConfig GetLevelConfig(int levelId)
    {
        for (var i = 0; i < levelConfigList.Length; ++i)
        {
            var levelConfig = levelConfigList[i];
            if (levelConfig.levelData.levelId == levelId)
            {
                return levelConfig;
            }
        }

        return null;
    }
}