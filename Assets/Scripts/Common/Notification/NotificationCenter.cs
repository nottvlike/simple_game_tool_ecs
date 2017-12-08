using System;
using System.Collections.Generic;

public struct NotificationSubTypeData : IEquatable<NotificationSubTypeData>
{
    public int subType;
    public NotificationMode mode;

    public bool Equals(NotificationSubTypeData other)
    {
        return subType == other.subType;
    }
}

public class NotificationCenter : INotificationCenter
{
    Dictionary<int, List<BaseNotification>> _notificationDict = new Dictionary<int, List<BaseNotification>>();
#if UNITOR_EDITOR
    Dictionary<int, List<NotificationSubTypeData>> _notificationTypeDict = new Dictionary<int, List<NotificationSubTypeData>>();
#endif

    public void Add(BaseNotification notification)
    {
#if UNITYR_EDITOR
        List<NotificationSubTypeData> subNotificationDataList;
        if (_notificationTypeDict.TryGetValue(notification.Type, out subNotificationDataList))
        {
            for (var i = 0; i < subNotificationDataList.Count; i++)
            {
                var subNotificationData = subNotificationDataList[i];
                if (subNotificationData.subType == notification.SubType 
                    && subNotificationData.mode != notification.Mode)
                {
                    LogUtil.E("Notification {0} add failed, NotificationMode is wrong!", notification.ToString());
                    break;
                }
            }
        }
#endif

        List<BaseNotification> notificationList;
        if (!_notificationDict.TryGetValue(notification.Type, out notificationList))
        {
            notificationList = new List<BaseNotification>();
            _notificationDict.Add(notification.Type, notificationList);
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
#if UNITYR_EDITOR

        List<NotificationSubTypeData> subNotificationDataList;
        if (_notificationTypeDict.TryGetValue(notification.Type, out subNotificationDataList))
        {
            for (var i = 0; i < subNotificationDataList.Count; i++)
            {
                var subNotificationData = subNotificationDataList[i];
                if (subNotificationData.subType == notification.SubType 
                    && subNotificationData.mode != notification.Mode)
                {
                    LogUtil.E("Notification {0} remove failed, NotificationMode is wrong!", notification.ToString());
                    break;
                }
            }
        }

#endif

        List<BaseNotification> notificationList;
        _notificationDict.TryGetValue(notification.Type, out notificationList);

#if UNITYR_EDITOR

        if (notificationList == null || notificationList.IndexOf(notification) == -1)
        {
            LogUtil.E("Could not find notification {0} in NotificationManager!", notification.ToString());
            return;
        }

#endif

        notificationList.Remove(notification);
    }

    public void Register(int type, int subType, NotificationMode mode)
    {
#if UNITOR_EDITOR
        List<NotificationSubTypeData> subNotificationList;
        if (!_notificationTypeDict.TryGetValue(type, out subNotificationList))
        {
            subNotificationList = new List<NotificationSubTypeData>();
        }

        for (var i = 0; i < subNotificationList.Count; i++)
        {
            var subNotificationData = subNotificationList[i];
            if (subNotificationData.subType == subType)
            {
                LogUtil.W("The notification subType {0} has been registered!", subType.ToString());
            }
        }

        var notificationSubData = new NotificationSubTypeData();
        notificationSubData.subType = subType;
        notificationSubData.mode = mode;
#endif
    }

    public void Notificate(NotificationData notificationData)
    {
        List<BaseNotification> notificationList;
        if (!_notificationDict.TryGetValue(notificationData.type, out notificationList))
        {
            return;
        }

        for (var i = 0; i < notificationList.Count; i++)
        {
            var notification = notificationList[i];
            if (notification.CanNotificate(notificationData.subType, notificationData.state))
            {
                if (notificationData.mode == NotificationMode.Object)
                {
                    var objectNotification = notification as ObjectNotification;
                    objectNotification.OnReceive(notificationData.data1);
                }
                else
                {
                    var valueTypeNotification = notification as ValueTypeNotification;
                    valueTypeNotification.OnReceive(notificationData.data2);
                }
            }
        }
    }

    public void Destroy()
    {
    }
}
