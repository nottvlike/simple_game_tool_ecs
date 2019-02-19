using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public enum BuffType
    {
        NormalChangeHp,
        ChangeHpMax,
        AddForce
    }

    public enum BuffValueType
    {
        Normal,
        Percentage
    }

    public enum BuffState
    {
        Init,
        Start,
        Stop,
        Finished
    }

    public enum BuffAttribute
    {
        None = 0,
        NeedRemove = 1,
    }

    [System.Serializable]
    public struct Buff
    {
        public int id;

        public BuffType buffType;
        public BuffValueType valueType;
        public int[] value;

        public int delay;

        public int time;
        public int count;
        [System.NonSerialized]
        public int currentCount;

        public int interval;
        [System.NonSerialized]
        public int lastUpdateTime;

        [System.NonSerialized]
        public BuffState buffState;

        public int buffAttribute;
    }

    [System.Serializable]
    public struct ActorConditionInfo
    {
        public int preloadId;
        [System.NonSerialized]
        public bool isDied;
    }

    [System.Serializable]
    public struct BattleVictoryContidion
    {
        public int limitTime;
        public ActorConditionInfo[] needDieActorInfoList;
    }

    public class BattleData : Data
    {
        public BattleInitialize battleInitialize;

        public override void Clear()
        {
            battleInitialize = null;
        }
    }

    public class BattleResourceData : Data
    {
        public Dictionary<GameObject, int> hurtDictionary = new Dictionary<GameObject, int>();
        public Dictionary<GameObject, int> attackDictionary = new Dictionary<GameObject, int>();

        public override void Clear()
        {
            hurtDictionary.Clear();
            attackDictionary.Clear();
        }
    }

    [System.Serializable]
    public struct ResourceHurtInfo
    {
        public int id;
        public Vector3Int force;
        public int duration;
    }

    public class ResourceHurtData : Data
    {
        public int attackObjDataId;
        public ResourceHurtInfo hurtInfo;
        public GameObject hurt;

        public override void Clear()
        {
            attackObjDataId = 0;
            hurtInfo.id = 0;
            hurtInfo.force = Vector3Int.zero;
            hurtInfo.duration = 0;
        }
    }

    [System.Serializable]
    public struct ResourceAttackInfo
    {
        public int id;
        public int[] buffList;
        public bool initial;
    }

    public class ResourceAttackData : Data
    {
        public GameObject attack;
        public ResourceAttackInfo attackInfo;

        public override void Clear()
        {
            attack = null;
            attackInfo.id = 0;
            attackInfo.buffList = null;
            attackInfo.initial = false;
        }
    }
}