using System;
using System.Collections.Generic;

public enum NotificationStateType
{
    None,
    Prepare,
    Run,
    Success,
    Failed,
}

public enum NotificationMode
{
    None,
    Object,
    ValueType
}

public struct NotificationData : IEquatable<NotificationData>
{
    public int type;
    public int subType;
    public NotificationStateType state;
    public NotificationMode mode;
    public object data1;
    public ValueType data2;

    public bool Equals(NotificationData other)
    {
        return type == other.type && subType == other.subType;
    }
}

public interface INotificationCenter
{
    void Register(int type, int subType, NotificationMode mode);
    void Add(BaseNotification notification);
    void Remove(BaseNotification notification);
    void Notificate(NotificationData notificationData);
    void Destroy();
}
