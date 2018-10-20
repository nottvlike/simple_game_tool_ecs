using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public enum JoyStickActionType
    {
        Run = 0,
        CancelRun,
        Jump,
        SkillDefault,
        SkillCustom
    }

    [Serializable]
    public enum JoyStickActionFaceType
    {
        None = 0,
        Left,
        Right
    }

    public struct JoyStickActionData
    {
        public int frame;
        public JoyStickActionType actionType;
        public JoyStickActionFaceType actionParam;
    }

    public class JoyStickData : Data
    {
        public List<JoyStickActionData> clientActionList;
        public List<JoyStickActionData> serverActionList;
    }
}