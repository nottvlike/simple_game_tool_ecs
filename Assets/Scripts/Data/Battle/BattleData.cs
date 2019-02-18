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

    public class BattleData : Data
    {
        public Dictionary<GameObject, int> hurtDictionary = new Dictionary<GameObject, int>();
        public Dictionary<GameObject, int> attackDictionary = new Dictionary<GameObject, int>();

        public override void Clear()
        {
            hurtDictionary.Clear();
            attackDictionary.Clear();
        }
    }
}