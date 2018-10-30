using System;

public delegate void OnValueTypeModeReceived(int type, ValueType notificationData);

public abstract class ValueTypeNotification : BaseNotification
{
    OnValueTypeModeReceived _onReceive;

    public ValueTypeNotification(OnValueTypeModeReceived onReceive = null)
        : base()
    {
        _onReceive = onReceive;
        _mode = NotificationMode.ValueType;
    }

    public virtual void OnReceive(int type, ValueType notificationData)
    {
        if (_onReceive != null)
        {
            _onReceive(type, notificationData);
        }
    }
}
