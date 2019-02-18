using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class BuffConfig : ScriptableObject
{
    public Buff[] buffList;
    public ActorHurtInfo[] hurtList;

    Buff _defaultBuff;
    ActorHurtInfo _defaultHurtInfo;

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

    public ActorHurtInfo GetHurtInfo(int hurtId)
    {
        for (var i = 0; i < hurtList.Length; i++)
        {
            var hurt = hurtList[i];
            if (hurt.id == hurtId)
            {
                return hurt;
            }
        }

        return _defaultHurtInfo;
    }
}
