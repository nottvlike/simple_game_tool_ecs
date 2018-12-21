using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public enum JoyStickActionType
    {
        None = 0,
        Run,
        CancelRun,
        Jump,
        SkillDefault,
        SkillCustom
    }

    public enum JoyStickActionFaceType
    {
        None = 0,
        Left,
        Right
    }

    public class JoyStickActionData : IPoolObject
    {
        public int frame;
        public JoyStickActionType actionType;
        public JoyStickActionFaceType actionParam;

        public bool IsInUse
        {
            get; set;
        }

        public void Clear()
        {
            frame = 0;
            actionType = JoyStickActionType.None;
            actionParam = JoyStickActionFaceType.None;
        }
    }

    public class JoyStickData : Data
    {
        public List<JoyStickActionData> clientActionList = new List<JoyStickActionData>();
        public List<JoyStickActionData> serverActionList = new List<JoyStickActionData>();
    }
}