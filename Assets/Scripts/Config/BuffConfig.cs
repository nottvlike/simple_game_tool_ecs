using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

[System.Serializable]
public struct Effect
{
    public int id;
    public int[] selfBuffIdList;
    public int[] friendBuffIdList;
    public int[] enemyBuffIdList;
    public int duration;
}

public class BuffConfig : ScriptableObject
{
    public Buff[] buffList;
    public Effect[] effectList;

    public Buff? GetBuff(int buffId)
    {
        for (var i = 0; i < buffList.Length; i++)
        {
            var buff = buffList[i];
            if (buff.id == buffId)
            {
                return buff;
            }
        }
        return null;
    }

    public Effect? GetEffect(int effectId)
    {
        for (var i = 0; i < effectList.Length; i++)
        {
            var effect = effectList[i];
            if (effect.id == effectId)
            {
                return effect;
            }
        }

        return null;
    }
}
