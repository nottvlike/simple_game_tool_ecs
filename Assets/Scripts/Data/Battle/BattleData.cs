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

        public Vector3Int forceValue;

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

    public class ResourceHurtData : Data
    {
        public int attackObjDataId;
        public Vector3Int force;
        public GameObject hurt;

        public override void Clear()
        {
            attackObjDataId = 0;
            hurt = null;
        }
    }

    public class ResourceAttackData : Data
    {
        public GameObject attack;
        public Effect attackEffect;

        public override void Clear()
        {
            attack = null;
            attackEffect.id = 0;
            attackEffect.selfBuffIdList = null;
            attackEffect.friendBuffIdList = null;
            attackEffect.enemyBuffIdList = null;
            attackEffect.initial = false;

        }
    }
}