using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class ActorData : Data
    {
        public int actorId;
        public int mass;
        public Vector3Int ground;
        public int gravity;
        public Vector3Int force;
    }

    public class ActorJumpData : Data
    {
        public Vector3Int currentJump;
        public int friction;
    }

    public class FollowCameraData : Data
    {
    }
}