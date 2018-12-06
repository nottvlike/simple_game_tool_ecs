using System;

public abstract class BaseNotification
{
    protected int _id;
    protected int[] _typeList;
    
    bool _enabled;

    public int Id
    {
        get { return _id; }
    }

    public int[] TypeList
    {
        get { return _typeList; }
    }

    public bool Enabled
    {
        get { return _enabled; }
        set
        {
            if (_id == 0)
            {
                LogUtil.E("Notification Id is 0, could not enable notification!");
                return;
            }

            if (_enabled == value)
            {
                return;
            }

            _enabled = value;
            if (_enabled)
            {
                WorldManager.Instance.NotificationCenter.Add(this);
            }
            else
            {
                WorldManager.Instance.NotificationCenter.Remove(this);
            }
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

    public virtual void OnReceive(int type, object notificationData)
    {
    }

    public virtual void OnReceive(int type, ValueType notificationData)
    {
    }
}
