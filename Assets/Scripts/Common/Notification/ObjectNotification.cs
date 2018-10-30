using System;

public delegate void OnObjectTypeModeReceived(int type, object notificationData);

public abstract class ObjectNotification : BaseNotification
{
    public OnObjectTypeModeReceived _onReceive;

    public ObjectNotification(OnObjectTypeModeReceived onReceive = null)
        : base()
    {
        _onReceive = onReceive;
        _mode = NotificationMode.Object;
    }

    public virtual void OnReceive(int type, object notificationData)
    {
        if (_onReceive != null)
        {
            _onReceive(type, notificationData);
        }
    }
}
