﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public enum ActorStateType
    {
        Idle = 1,
        Move = 2,
        Jump = 4,
        SkillDefault = 8,
        SkillCustom = 16,
        Hurt = 32
    }

    public enum ActorCampType
    {
        Player,
        Friend,
        Enemy,
        Neutral
    }

    public class ActorData : Data
    {
        public int actorId;
        public int currentState;
        public ActorCampType camp;
        public SkillDefaultType defaultSkill;

        public override void Clear()
        {
            actorId = 0;
            currentState = 0;
            camp = ActorCampType.Player;
            defaultSkill = SkillDefaultType.None;
        }
    }

    public class ActorController2DData : Data
    {
        public Rigidbody2D rigidbody2D;
        public Transform root;
        public Transform foot;
        public Transform hurt;
        public GameObject attack;
        public int groundY;
        public int positionY;

        public override void Clear()
        {
            rigidbody2D = null;
            root = null;
            foot = null;
            hurt = null;
            attack = null;
            groundY = 0;
            positionY = 0;
        }
    }

    public class ActorFlyData : Data
    {
        public int duration;
        public int currentDuration;

        public override void Clear()
        {
            duration = 0;
            currentDuration = 0;
        }
    }

    public class ActorDashData : Data
    {
        public int duration;
        public int currentDuration;

        public override void Clear()
        {
            duration = 0;
            currentDuration = 0;
        }
    }

    public class ActorStressData : Data
    {
        public int duration;
        public int currentDuration;

        public override void Clear()
        {
            duration = 0;
            currentDuration = 0;
        }
    }

    public class ActorSyncData : Data
    {
    }

    public class FollowCameraData : Data
    {
    }

    public class ActorBuffData : Data
    {
        public List<Buff> buffList = new List<Buff>();

        public override void Clear()
        {
            buffList.Clear();
        }
    }

    [System.Serializable]
    public struct BaseAttributeInfo
    {
        public int hp;
        public int mp;
        public int atk;
        public int def;
    }

    public struct ExtraAttributeInfo
    {
        public int hp;
        public int hpMax;
        public int mp;
        public int atk;
        public int def;
    }

    public class ActorAttributeData : Data
    {
        public BaseAttributeInfo baseAttribute;
        public ExtraAttributeInfo extraAttribute;

        public override void Clear()
        {
            baseAttribute.hp = 0;
            baseAttribute.mp = 0;
            baseAttribute.atk = 0;
            baseAttribute.def = 0;
            extraAttribute.hp = 0;
            extraAttribute.hpMax = 0;
            extraAttribute.mp = 0;
            extraAttribute.atk = 0;
            extraAttribute.def = 0;
        }
    }

    [System.Serializable]
    public struct ActorHurtInfo
    {
        public int id;
        public Vector3Int force;
        public int duration;
    }

    public class ActorHurtData : Data
    {
        public int attackObjDataId;
        public ActorHurtInfo hurt;

        public override void Clear()
        {
            attackObjDataId = 0;
            hurt.id = 0;
            hurt.force = Vector3Int.zero;
            hurt.duration = 0;
        }
    }
}