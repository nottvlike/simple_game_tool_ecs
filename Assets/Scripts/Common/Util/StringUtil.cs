using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringUtil
{
    public static string Get(string id)
    {
        var localizationData = WorldManager.Instance.GameCore.GetData<Data.GameLocalizationData>();
        if (localizationData.currentConfig == null)
        {
            InitLocalizationConfig(localizationData);
        }

        return localizationData.currentConfig.Get(id);
    }

    static void InitLocalizationConfig(Data.GameLocalizationData data)
    {
        var localizationConfigList = WorldManager.Instance.LocalizationConfigGroup.localizationConfigList;
        for (var i = 0; i < localizationConfigList.Length; i++)
        {
            var localizationConfig = localizationConfigList[i];
            if (localizationConfig.zone == data.zone)
            {
                localizationConfig.Init();
                data.currentConfig = localizationConfig;
                return;
            }
        }

        LogUtil.E("Failed to find localization config " + data.zone);
    }
}
