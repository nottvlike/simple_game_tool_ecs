using System;

public class ValueTypeNotification : BaseNotification
{
    public ValueTypeNotification()
        : base()
    {
        _mode = NotificationMode.ValueType;
    }

    public virtual void OnReceive(ValueType notificationData)
    {
        LogUtil.E("You should rewrite the OnValueTypeReceive method!");
    }
}
