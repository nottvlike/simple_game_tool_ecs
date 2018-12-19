using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public struct StringObject
{
    public string id;
    public string content;
}

public class LocalizationConfig : ScriptableObject
{
    public string zone;
    public StringObject[] stringList;

    Dictionary<string, string> _stringDictionary = new Dictionary<string, string>();
    public void Init()
    {
        for (var i = 0; i < stringList.Length; i++)
        {
            var stringObject = stringList[i];
            _stringDictionary.Add(stringObject.id, stringObject.content);
        }
    }

    public string Get(string id)
    {
        string result;
        if (_stringDictionary.TryGetValue(id, out result))
        {
            return result;
        }

        LogUtil.E("Failed to find string {0}", id);
        return id;
    }
}
