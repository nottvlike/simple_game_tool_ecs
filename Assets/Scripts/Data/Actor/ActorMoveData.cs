using UnityEngine;

namespace Data
{
    public class PositionData : Data
    {
        public Vector3Int position;
    }

    public class DirectionData : Data
    {
        public Vector3Int direction;
    }

    public class SpeedData : Data
    {
        public int speed;
        public int friction;
    }
}