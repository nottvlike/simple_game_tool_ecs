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
public struct ResourcePreloadInfo
{
    public ResourcePreloadType type;
    public string[] resourceNameList;
}

public class ResourcePreloadConfig : ScriptableObject
{
    public ResourcePreloadInfo[] resourcePreloadInfoList;

    public string[] GetResourceNameList(ResourcePreloadType preloadType)
    {
        for (var i = 0; i < resourcePreloadInfoList.Length; i++)
        {
            var resourcePreloadInfo = resourcePreloadInfoList[i];
            if (resourcePreloadInfo.type == preloadType)
            {
                return resourcePreloadInfo.resourceNameList;
            }
        }

        LogUtil.E("Failed to find Config for ResourcePreloadType " + preloadType);
        return null;
    }
}
