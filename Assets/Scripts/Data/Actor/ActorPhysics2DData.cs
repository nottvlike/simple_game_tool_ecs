﻿using UnityEngine;

namespace Data
{
    public class Physics2DData : Data
    {
        public int mass;
        public int gravity;
        public Vector3Int force;
        public int friction;
        public int airFriction;
    }

    public class DirectionData : Data
    {
        public Vector3Int direction;
    }

    public class SpeedData : Data
    {
        public int speed;
    }

    public class ActorJumpData : Data
    {
        public Vector3Int currentJump;
    }
}