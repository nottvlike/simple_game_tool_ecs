using System;
using System.Collections.Generic;

public partial class WorldManager : Singleton<WorldManager>
{
    INotificationCenter _notificationCenter;
    public INotificationCenter NotificationCenter
    {
        get
        {
            if (_notificationCenter == null)
                _notificationCenter = new NotificationCenter();

            return _notificationCenter;
        }
    }

    void DestroyNotificationCenter()
    {
        _notificationCenter.Destroy();
        _notificationCenter = null;
    }
}
