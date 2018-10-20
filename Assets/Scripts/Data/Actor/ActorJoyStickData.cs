using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public enum JoyStickActionType
    {
        Run = 0,
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

    public struct JoyStickActionData
    {
        public int frame;
        public JoyStickActionType actionType;
        public JoyStickActionFaceType actionParam;
    }

    public class JoyStickData : Data
    {
        public List<JoyStickActionData> clientActionList = new List<JoyStickActionData>();
        public List<JoyStickActionData> serverActionList = new List<JoyStickActionData>();
    }
}