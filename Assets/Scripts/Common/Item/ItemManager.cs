using System;
using System.Collections.Generic;

public enum ItemType
{
    Normal,     // 普通道具
    Point,      // 在背包看不到却可以消耗的道具
}

public enum ItemNormalType
{
    Equip,
    Stone,
    Food,
    Other
}

public enum ItemPointType
{
    Gold,       // 一级货币
    Silver,     // 二级货币
    Stamina,    // 体力
}

public enum ItemNotificationType
{
    AmountChange,                  // 物品数量改变（包括可叠加和不可叠加）
    NoOverlayItemInfoChange,       // 非叠加物品信息改变
}

public struct ItemData : IEquatable<ItemData>
{
    public int id;
    public int type;
    public int subType;
    public int status;          //可叠加等其它状态

    public int name;
    public int description;

    public string iconName;
    public string smallIconName;

    public bool Equals(ItemData other)
    {
        return id == other.id;
    }
}

public struct ItemInfo : IEquatable<ItemInfo>
{
    public int id;          
    public int uniqueId;            // 不可叠加的物品需要一个 unique id 来区分
    public int level;
    public int experience;

    public bool Equals(ItemInfo other)
    {
        return uniqueId == other.uniqueId;
    }
}

public struct OverlayItemInfo : IEquatable<OverlayItemInfo>
{
    public int id;
    public int count;

    public bool Equals(OverlayItemInfo other)
    {
        return id == other.id;
    }
}

public class ItemManager
{
    const int STATUS_OVERLAY = 0x01;

    Dictionary<int, ItemData> _itemDataDict = new Dictionary<int, ItemData>();
    Dictionary<int, ItemInfo> _itemInfoDict = new Dictionary<int, ItemInfo>();
    Dictionary<int, OverlayItemInfo> _overlayItemInfoDict = new Dictionary<int, OverlayItemInfo>();

    public void Init()
    {
#if UNITOR_EDITOR
        var notificationCenter = WorldManager.Instance.GetNotificationCenter();
        notificationCenter.Register(Constant.NOTIFICATION_TYPE_ITEM, (int)ItemNotificationType.AmountChange, NotificationMode.ValueType);
        notificationCenter.Register(Constant.NOTIFICATION_TYPE_ITEM, (int)ItemNotificationType.NoOverlayItemInfoChange, NotificationMode.ValueType);
#endif
    }

    public ItemData GetItemData(int itemId)
    {
        return Constant.defaultItemData;
    }

    public int GetItemCount(int itemId)
    {
        return 0;
    }

    // 服务器下发道具信息
    void UpdateItem(int itemId, int itemUniqueId, int itemLevel, int itemExperience)
    {
        var notificationCenter = WorldManager.Instance.GetNotificationCenter();

        ItemInfo itemInfo;
        if (_itemInfoDict.TryGetValue(itemUniqueId, out itemInfo))
        {
            itemInfo.level = itemLevel;
            itemInfo.experience = itemExperience;
            itemInfo.id = itemId;

            var notificationData = Constant.defaultNotificationData;
            notificationData.type = Constant.NOTIFICATION_TYPE_ITEM;
            notificationData.subType = (int)ItemNotificationType.NoOverlayItemInfoChange;
            notificationData.mode = NotificationMode.ValueType;
            notificationData.state = NotificationStateType.None;
            notificationData.data2 = itemInfo;
            notificationCenter.Notificate(notificationData);
        }
        else
        {
            itemInfo = new ItemInfo { id = itemId, uniqueId = itemUniqueId, level = itemLevel, experience = itemExperience };
            _itemInfoDict.Add(itemUniqueId, itemInfo);

            var notificationData = Constant.defaultNotificationData;
            notificationData.type = Constant.NOTIFICATION_TYPE_ITEM;
            notificationData.subType = (int)ItemNotificationType.AmountChange;
            notificationData.mode = NotificationMode.ValueType;
            notificationData.state = NotificationStateType.None;
            notificationData.data2 = itemId;
            notificationCenter.Notificate(notificationData);
        }
    }

    // 服务器下发道具信息
    void UpdateOverlayItem(int itemId, int itemCount)
    {
        OverlayItemInfo itemInfo;
        if (_overlayItemInfoDict.TryGetValue(itemId, out itemInfo))
        {
            itemInfo.count = itemCount;
        }
        else
        {
            itemInfo = new OverlayItemInfo { id = itemId, count = itemCount };
            _overlayItemInfoDict.Add(itemId, itemInfo);
        }

        var notificationData = Constant.defaultNotificationData;
        notificationData.type = Constant.NOTIFICATION_TYPE_ITEM;
        notificationData.subType = (int)ItemNotificationType.AmountChange;
        notificationData.mode = NotificationMode.ValueType;
        notificationData.state = NotificationStateType.None;
        notificationData.data2 = itemId;
        WorldManager.Instance.GetNotificationCenter().Notificate(notificationData);
    }
}
