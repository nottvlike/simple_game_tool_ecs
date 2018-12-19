using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct PanelInfo : IEquatable<PanelInfo>
{
    public PanelType panelType;
    public PanelMode panelMode;
    public PanelGroup panelGroup;
    public PanelType rootPanelType;
    public string resourceName;

    public bool Equals(PanelInfo other)
    {
        return panelType == other.panelType;
    }
}

public class PanelConfig : ScriptableObject
{
    public PanelInfo[] panelConfigList;

    Dictionary<PanelType, PanelInfo> _panelConfigDict;
    public Dictionary<PanelType, PanelInfo> PanelConfigDict
    {
        get
        {
            if (_panelConfigDict == null)
            {
                _panelConfigDict = new Dictionary<PanelType, PanelInfo>();
                for (var i = 0; i < panelConfigList.Length; i++)
                {
                    var panelData = panelConfigList[i];
                    _panelConfigDict.Add(panelData.panelType, panelData);
                }
            }

            return _panelConfigDict;
        }
    }
}
