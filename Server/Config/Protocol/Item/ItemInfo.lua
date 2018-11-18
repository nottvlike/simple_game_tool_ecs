-- automatically generated by the FlatBuffers compiler, do not modify

-- namespace: Item

local flatbuffers = require('flatbuffers')

local ItemInfo = {} -- the module
local ItemInfo_mt = {} -- the class metatable

function ItemInfo.New()
    local o = {}
    setmetatable(o, {__index = ItemInfo_mt})
    return o
end
function ItemInfo.GetRootAsItemInfo(buf, offset)
    local n = flatbuffers.N.UOffsetT:Unpack(buf, offset)
    local o = ItemInfo.New()
    o:Init(buf, n + offset)
    return o
end
function ItemInfo_mt:Init(buf, pos)
    self.view = flatbuffers.view.New(buf, pos)
end
function ItemInfo_mt:ItemId()
    local o = self.view:Offset(4)
    if o ~= 0 then
        return self.view:Get(flatbuffers.N.Int32, o + self.view.pos)
    end
    return 0
end
function ItemInfo_mt:ItemType()
    local o = self.view:Offset(6)
    if o ~= 0 then
        return self.view:Get(flatbuffers.N.Int32, o + self.view.pos)
    end
    return 0
end
function ItemInfo_mt:ItemCount()
    local o = self.view:Offset(8)
    if o ~= 0 then
        return self.view:Get(flatbuffers.N.Int32, o + self.view.pos)
    end
    return 0
end
function ItemInfo.Start(builder) builder:StartObject(3) end
function ItemInfo.AddItemId(builder, itemId) builder:PrependInt32Slot(0, itemId, 0) end
function ItemInfo.AddItemType(builder, itemType) builder:PrependInt32Slot(1, itemType, 0) end
function ItemInfo.AddItemCount(builder, itemCount) builder:PrependInt32Slot(2, itemCount, 0) end
function ItemInfo.End(builder) return builder:EndObject() end

return ItemInfo -- return the module