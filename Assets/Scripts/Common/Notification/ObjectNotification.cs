using System;

public abstract class ObjectNotification : BaseNotification
{
    public ObjectNotification()
        : base()
    {
        _mode = NotificationMode.Object;
    }

    public abstract void OnReceive(object notificationData);
}
