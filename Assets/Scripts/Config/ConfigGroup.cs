using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigGroup : ScriptableObject
{
    public ScriptableObject[] scriptableObjectList;

    public T Get<T>() where T: ScriptableObject
    {
        for (var i = 0; i < scriptableObjectList.Length; i++)
        {
            var scriptableObject = scriptableObjectList[i];
            if (scriptableObject.GetType() == typeof(T))
                return scriptableObject as T;
        }

        LogUtil.W("Failed to find config " + typeof(T));
        return null;
    }
}
