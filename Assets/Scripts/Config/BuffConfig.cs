using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class BuffConfig : ScriptableObject
{
    public Buff[] buffList;
    public ResourceHurtInfo[] hurtList;
    public ResourceAttackInfo[] attackList;

    Buff _defaultBuff;
    ResourceHurtInfo _defaultHurtInfo;
    ResourceAttackInfo _defaultAttackInfo;

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

    public ResourceHurtInfo GetHurtInfo(int hurtId)
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

    public ResourceAttackInfo GetAttackInfo(int attackId)
    {
        for (var i = 0; i < attackList.Length; i++)
        {
            var attack = attackList[i];
            if (attack.id == attackId)
            {
                return attack;
            }
        }

        return _defaultAttackInfo;
    }
}
