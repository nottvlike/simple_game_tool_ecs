using System;

public abstract class BaseNotification
{
    protected int _id;
    protected int[] _typeList;

    protected NotificationMode _mode = NotificationMode.None;

    public int Id
    {
        get { return _id; }
    }

    public int[] TypeList
    {
        get { return _typeList; }
    }

    public NotificationMode Mode
    {
        get { return _mode; }
    }

    public void Start()
    {
        if (_id != 0)
        {
            WorldManager.Instance.GetNotificationCenter().Add(this);
        }
    }

    public void Stop()
    {
        if (_id != 0)
        {
            WorldManager.Instance.GetNotificationCenter().Remove(this);
        }
    }

    public virtual bool CanNotificate(int type)
    {
        if (type == 0)
        {
            return true;
        }

        for (var i = 0; i < _typeList.Length; i++)
        {
            if (_typeList[i] == type)
            {
                return true;
            }
        }

        return false;
    }
}
