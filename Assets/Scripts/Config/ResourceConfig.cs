using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

