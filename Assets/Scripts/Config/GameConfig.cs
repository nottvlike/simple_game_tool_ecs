using System;
using System.Collections.Generic;
using UnityEngine;
using Data;

[Serializable]
public class PlayerData
{
    public int id;
    public string resource;

    public PositionData positionData;
    public DirectionData directionData;
    public SpeedData speedData;
}

[Serializable]
public class TalkingData
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
public class DramaData
{
    public DramaItemType dramaItemType;
    public int id;
}

[Serializable]
public class LevelData
{
    public int levelId;
    public string levelName;

    public List<DramaData> dramaItemList;
}

[Serializable]
public class LevelConfig
{
    public PlayerData playerData;
    public TalkingData[] talkingData;

    public LevelData levelData;
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