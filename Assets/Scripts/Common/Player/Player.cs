using System;

public struct BasicPlayerData : IEquatable<BasicPlayerData>
{
    public long accountId;
    public string name;
    public int level;
    public int experience;
    public int vip;
    public long createTime;

    public bool Equals(BasicPlayerData other)
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
    BasicPlayerData _basicData;
}
