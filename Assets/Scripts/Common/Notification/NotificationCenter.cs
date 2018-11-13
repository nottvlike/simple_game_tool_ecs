using System;
using System.Collections.Generic;

public class NotificationCenter : INotificationCenter
{
    Dictionary<int, List<BaseNotification>> _notificationDict = new Dictionary<int, List<BaseNotification>>();

    public void Add(BaseNotification notification)
    {
        List<BaseNotification> notificationList;
        if (!_notificationDict.TryGetValue(notification.Id, out notificationList))
        {
            notificationList = new List<BaseNotification>();
            _notificationDict.Add(notification.Id, notificationList);
        }

#if UNITYR_EDITOR
        if (notificationList.IndexOf(notification) != -1)
        {
            LogUtil.E("Notification {0} has been added to the NotificationManager!", notification.ToString());
            return;
        }
#endif

        notificationList.Add(notification);
    }

    public void Remove(BaseNotification notification)
    {
        List<BaseNotification> notificationList;
        _notificationDict.TryGetValue(notification.Id, out notificationList);

#if UNITYR_EDITOR
        if (notificationList == null || notificationList.IndexOf(notification) == -1)
        {
            LogUtil.E("Could not find notification {0} in NotificationManager!", notification.ToString());
            return;
        }
#endif

        notificationList.Remove(notification);
    }

    public void Notificate(NotificationData notificationData)
    {
        List<BaseNotification> notificationList;
        if (!_notificationDict.TryGetValue(notificationData.id, out notificationList))
        {
            return;
        }

        for (var i = 0; i < notificationList.Count; i++)
        {
            var notification = notificationList[i];
            if (notification.CanNotificate(notificationData.type))
            {
                if (notificationData.mode == NotificationMode.Object)
                {
                    var objectNotification = notification as ObjectNotification;
                    objectNotification.OnReceive(notificationData.type, notificationData.data1);
                }
                else
                {
                    var valueTypeNotification = notification as ValueTypeNotification;
                    valueTypeNotification.OnReceive(notificationData.type, notificationData.data2);
                }
            }
        }
    }

    public void Destroy()
    {
    }
}
