using System;

namespace Data
{
    [Serializable]
    public class PositionData : Data
    {
        public int x;
        public int y;
        public int z;
    }

    [Serializable]
    public class DirectionData : Data
    {
        public int x;
        public int y;
        public int z;
    }

    [Serializable]
    public class SpeedData : Data
    {
        public int noraml;
        public int max;
        public int delta;
        public int lastAcceleration;
        public int acceleration;
        public int accelerationDelta;
    }
}