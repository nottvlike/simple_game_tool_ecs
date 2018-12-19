using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourcePreloadType
{
    GameInit,
    EnterMain,
    EnterFight
}

[System.Serializable]
public struct ResourcePreloadData
{
    public ResourcePreloadType type;
    public string[] resourceNameList;
}

public class ResourcePreloadConfig : ScriptableObject
{
    public ResourcePreloadData[] resourcePreloadDataList;
}
