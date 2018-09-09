using System;

[Serializable]
public class PositionData : IData
{
    public int x;
    public int y;
    public int z;

    public PositionData Clone()
    {
        return (PositionData)this.MemberwiseClone();
    }
}

[Serializable]
public class DirectionData : IData
{
    public int x;
    public int y;
    public int z;

    public DirectionData Clone()
    {
        return (DirectionData)this.MemberwiseClone();
    }
}

[Serializable]
public class SpeedData : IData
{
    public int noraml;
    public int max;
    public int delta;
    public int lastAcceleration;
    public int acceleration;
    public int accelerationDelta;

    public SpeedData Clone()
    {
        return (SpeedData)this.MemberwiseClone();
    }
}