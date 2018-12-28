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
        public int halfWidth;
        public int halfHeight;
    }

    public class Collider2DData : Data
    {
        public Transform ground;
        public Transform forward;
        public Transform back;
    }

    public class PositionData : Data
    {
        public Vector3Int position;
        public Vector3Int ground;
        public Vector3Int forward;
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