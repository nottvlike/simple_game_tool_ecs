using System;
using FlatBuffers;

public enum ItemNotificationType
{
    CountChanged = 1,
}

public class ItemNotification : ValueTypeNotification
{
    NotificationData _notificationData;

    public ItemNotification()
    {
        _id = Constant.NOTIFICATION_TYPE_ITEM;
        _typeList = new int[2] { (int)Protocols.ResAllItemInfo, (int)Protocols.ResUpdateItemInfo };

        _notificationData = new NotificationData();
        _notificationData.id = Constant.NOTIFICATION_TYPE_ITEM;
        _notificationData.mode = NotificationMode.ValueType;

        Enabled = true;
    }

    public override void OnReceive(int type, ValueType notificationData)
    {
        var msg = (Message)notificationData;
        switch(type)
        {
            case (int)Protocols.ResAllItemInfo:
                ResAllItemInfo(msg);
                break;
            case (int)Protocols.ResUpdateItemInfo:
                ResUpdateItemInfo(msg);
                break;
            default:
                break;
        }
    }

    void ResAllItemInfo(Message msg)
    {
        var byteBuffer = new ByteBuffer(msg.data);
        var allItemInfo = Protocol.Item.ResAllItemInfo.GetRootAsResAllItemInfo(byteBuffer);

        var itemInfoData = WorldManager.Instance.Item.GetData<Data.ItemInfoData>();
        var itemInfoList = itemInfoData.itemInfoList;

        itemInfoList.Clear();
        for (var i = 0; i < allItemInfo.ItemInfoListLength; i++)
        {
            var itemInfo = allItemInfo.ItemInfoList(i);

            var item = new Data.ItemInfo();
            item.uniqueId = itemInfo.Value.ItemId;
            item.typeId = itemInfo.Value.ItemType;
            item.count = itemInfo.Value.ItemCount;
            itemInfoList.Add(item);
        }
    }

    void ResUpdateItemInfo(Message msg)
    {
        var byteBuffer = new ByteBuffer(msg.data);
        var updateItemInfoMsg = Protocol.Item.ResUpdateItemInfo.GetRootAsResUpdateItemInfo(byteBuffer);

        var worldMgr = WorldManager.Instance;
        var notificationCenter = worldMgr.NotificationCenter;
        var itemInfoData = worldMgr.Item.GetData<Data.ItemInfoData>();
        var itemInfoList = itemInfoData.itemInfoList;

        var updateItemInfo = updateItemInfoMsg.ItemInfo;

        var i = 0;
        for (; i < itemInfoList.Count; i++)
        {
            var itemInfo = itemInfoList[i];
            if (updateItemInfo.Value.ItemId == itemInfo.uniqueId)
            {
                itemInfo.typeId = updateItemInfo.Value.ItemType;
                itemInfo.count = updateItemInfo.Value.ItemCount;

                _notificationData.data2 = itemInfo;
                _notificationData.type = (int)ItemNotificationType.CountChanged;
                notificationCenter.Notificate(_notificationData);
                break;
            }
        }

        if (i == itemInfoList.Count)
        {
            var item = new Data.ItemInfo();
            item.uniqueId = updateItemInfo.Value.ItemId;
            item.typeId = updateItemInfo.Value.ItemType;
            item.count = updateItemInfo.Value.ItemCount;
            itemInfoList.Add(item);

            _notificationData.data2 = item;
            _notificationData.type = (int)ItemNotificationType.CountChanged;
            notificationCenter.Notificate(_notificationData);
        }
    }
}
