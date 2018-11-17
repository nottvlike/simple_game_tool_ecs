using System.Collections;
using System.Collections.Generic;

namespace Data
{
    public struct ItemInfo
    {
        public int uniqueId;
        public int typeId;
        public int count;
    }

    public class ItemInfoData : Data
    {
        public List<ItemInfo> itemInfoList = new List<ItemInfo>();
        public ItemNotification notification = new ItemNotification();
    }
}
