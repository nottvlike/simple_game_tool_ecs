using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringUtil
{
    public static string Get(string id)
    {
        var localizationData = WorldManager.Instance.GameCore.GetData<Data.GameLocalizationData>();
        return localizationData.currentConfig.Get(id);
    }
}
