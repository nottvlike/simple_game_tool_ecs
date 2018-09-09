using System;

public abstract class ValueTypeNotification : BaseNotification
{
    public ValueTypeNotification()
        : base()
    {
        _mode = NotificationMode.ValueType;
    }

    public abstract void OnReceive(ValueType notificationData);
}
