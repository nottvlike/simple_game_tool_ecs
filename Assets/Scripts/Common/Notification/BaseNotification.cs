using System;

public abstract class BaseNotification
{
    protected int _type;
    protected int _subType;
    protected NotificationMode _mode = NotificationMode.None;
    protected NotificationStateType _state = NotificationStateType.None;

    public NotificationStateType State
    {
        get { return _state; }
    }

    public int Type
    {
        get { return _type; }
    }

    public int SubType
    {
        get { return _subType; }
    }

    public NotificationMode Mode
    {
        get { return _mode; }
    }

    public void Start()
    {
        if (_type != 0)
        {
            WorldManager.Instance.GetNotificationCenter().Add(this);
        }
    }

    public void Stop()
    {
        if (_type != 0)
        {
            WorldManager.Instance.GetNotificationCenter().Remove(this);
        }
    }

    public virtual bool CanNotificate(int notificationSubType, NotificationStateType notificationState)
    {
        if (_subType == notificationSubType && _state == notificationState)
        {
            return true;
        }

        return false;
    }
}
