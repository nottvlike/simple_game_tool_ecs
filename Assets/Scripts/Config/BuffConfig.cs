using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class BuffConfig : ScriptableObject
{
    public Buff[] buffList;

    Buff _defaultBuff;

    public Buff Get(int buffId)
    {
        for (var i = 0; i < buffList.Length; i++)
        {
            var buff = buffList[i];
            if (buff.id == buffId)
            {
                return buff;
            }
        }
        return _defaultBuff;
    }
}
