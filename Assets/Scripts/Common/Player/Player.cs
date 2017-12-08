using System;

public struct BasicData : IEquatable<BasicData>
{
    public long accountId;
    public string name;
    public int level;
    public int experience;
    public int vip;
    public long createTime;

    public bool Equals(BasicData other)
    {
        return accountId == other.accountId;
    }
}

public enum PlayerNotificationType
{
    NameChange,
    LevelChange,
    ExperienceChange,
    VipChange,
}

public class Player
{
    BasicData _basicData;
}
