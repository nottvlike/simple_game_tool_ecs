using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public enum BuffType
    {
        NormalChangeHp,
        ChangeHpMax,
    }

    public enum BuffValueType
    {
        Normal,
        Percentage
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
    }

    public class BattleData : Data
    {
        public Dictionary<GameObject, int> hurtDictionary = new Dictionary<GameObject, int>();
    }
}