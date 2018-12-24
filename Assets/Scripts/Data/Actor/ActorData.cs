using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class ActorData : Data
    {
        public int actorId;
        public int mass;
    }

    public class ActorJumpData : Data
    {
        public Vector3 currentJump;
        public int fall;
        public int friction;
        public int gravity;
        public Vector3 groundPosition;
    }
}