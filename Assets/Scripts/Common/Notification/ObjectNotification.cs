using System;

public class ObjectNotification : BaseNotification
{
    public ObjectNotification()
        : base()
    {
        _mode = NotificationMode.Object;
    }

    public virtual void OnReceive(object notificationData)
    {
        LogUtil.E("You should rewrite the OnObjectReceive method!");
    }
}
