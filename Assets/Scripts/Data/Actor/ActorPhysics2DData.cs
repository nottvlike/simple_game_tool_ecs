using UnityEngine;

namespace Data
{
    public class Physics2DData : Data
    {
        public int mass;
        public int gravity;
        public Vector3Int force;
        public int friction;
        public int airFriction;

        public override void Clear()
        {
            mass = 0;
            gravity = 0;
            force = Vector3Int.zero;
            friction = 0;
            airFriction = 0;
        }
    }

    public class DirectionData : Data
    {
        public Vector3Int direction;

        public override void Clear()
        {
            direction = Vector3Int.zero;
        }
    }

    public class SpeedData : Data
    {
        public int speed;

        public override void Clear()
        {
            speed = 0;
        }
    }

    public class ActorJumpData : Data
    {
        public Vector3Int currentJump;

        public override void Clear()
        {
            currentJump = Vector3Int.zero;
        }
    }
}