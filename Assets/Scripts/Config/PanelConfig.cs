using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct PanelData : IEquatable<PanelData>
{
    public PanelType panelType;
    public PanelMode panelMode;
    public PanelGroup panelGroup;
    public PanelType rootPanelType;
    public string resourceName;

    public bool Equals(PanelData other)
    {
        return panelType == other.panelType;
    }
}

public class PanelConfig : ScriptableObject
{
    public PanelData[] panelConfigList;

    Dictionary<PanelType, PanelData> _panelConfigDict;
    public Dictionary<PanelType, PanelData> PanelConfigDict
    {
        get
        {
            if (_panelConfigDict == null)
            {
                _panelConfigDict = new Dictionary<PanelType, PanelData>();
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
