using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public enum ActorStateType
    {
        Idle,
        Move,
        Jump,
        SkillDefault,
        SkillCustom
    }

    public class ActorData : Data
    {
        public int actorId;
        public ActorStateType currentState;
    }

    public class ActorSyncData : Data
    {
    }

    public class FollowCameraData : Data
    {
    }
}