using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class ActorData : Data
    {
        public int actorId;
        public int mass;
        public Vector3 ground;
        public int gravity;
        public Vector3 force;
    }

    public class ActorJumpData : Data
    {
        public Vector3 currentJump;
        public int friction;
    }

    public class FollowCameraData : Data
    {
    }
}