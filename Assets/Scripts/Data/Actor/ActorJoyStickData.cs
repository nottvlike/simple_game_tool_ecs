using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public enum SkillDefaultType
    {
        None,
        Fly,
        Dash
    }

    public enum JoyStickActionType
    {
        None = 0,
        Move,
        CancelMove,
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
        public SkillDefaultType skillDefaultType;

        public bool IsInUse
        {
            get; set;
        }

        public void Clear()
        {
            frame = 0;
            actionType = JoyStickActionType.None;
            actionParam = JoyStickActionFaceType.None;
            skillDefaultType = SkillDefaultType.None;
        }
    }

    public class ClientJoyStickData : Data
    {
        public List<JoyStickActionData> actionList = new List<JoyStickActionData>();
    }

    public class ServerJoyStickData : Data
    {
        public List<JoyStickActionData> actionList = new List<JoyStickActionData>();
    }
}