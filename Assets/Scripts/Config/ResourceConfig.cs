using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

[Serializable]
public struct ResourceInfo
{
    public string resourceName;
    public string resourcePath;
    public ResourceType type;
    public string assetbundlePath;
    public bool isFromAssetBundle;

    public bool Equals(ResourceInfo other)
    {
        return resourceName == other.resourceName && resourcePath == other.resourcePath;
    }
}

public class ResourceConfig : ScriptableObject
{
    public ResourceInfo[] resourceDataList;

    Dictionary<string, ResourceInfo> _resourceDict = null;
    public Dictionary<string, ResourceInfo> ResourceDict
    {
        get
        {
            if (_resourceDict == null)
            {
                _resourceDict = new Dictionary<string, ResourceInfo>();
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

