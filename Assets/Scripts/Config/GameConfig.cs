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

[Serializable]
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

[Serializable]
public struct ResourceData
{
    public string resourceName;
    public string resourcePath;
    public string assetbundlePath;
    public bool isFromAssetBundle;

    public bool Equals(ResourceData other)
    {
        return resourceName == other.resourceName && resourcePath == other.resourcePath;
    }
}

public class ResourceConfig : ScriptableObject
{
    public ResourceData[] resourceDataList;

    Dictionary<string, ResourceData> _resourceDict = null;
    public Dictionary<string, ResourceData> ResourceDict
    {
        get
        {
            if (_resourceDict == null)
            {
                _resourceDict = new Dictionary<string, ResourceData>();
                for (var i = 0; i < resourceDataList.Length; i++)
                {
                    var resourceData = resourceDataList[i];
                    _resourceDict.Add(resourceData.resourceName, resourceData);
                }
            }

            return _resourceDict;
        }
    }
}

[Serializable]
public enum KeyStateType
{
    None = 0,
    Down,
    Up
}

[Serializable]
public struct JoyStickMapData
{
    public KeyCode[] keyCode;
    public KeyStateType keyStateType;
    public JoyStickActionType joyStickActionType;
    public JoyStickActionFaceType joyStickActionFaceType;
}

public class JoyStickConfig : ScriptableObject
{
    public JoyStickMapData[] joyStickMapDataList;
}

