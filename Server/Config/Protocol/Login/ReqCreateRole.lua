-- automatically generated by the FlatBuffers compiler, do not modify

-- namespace: Login

local flatbuffers = require('flatbuffers')

local ReqCreateRole = {} -- the module
local ReqCreateRole_mt = {} -- the class metatable

function ReqCreateRole.New()
    local o = {}
    setmetatable(o, {__index = ReqCreateRole_mt})
    return o
end
function ReqCreateRole.GetRootAsReqCreateRole(buf, offset)
    local n = flatbuffers.N.UOffsetT:Unpack(buf, offset)
    local o = ReqCreateRole.New()
    o:Init(buf, n + offset)
    return o
end
function ReqCreateRole_mt:Init(buf, pos)
    self.view = flatbuffers.view.New(buf, pos)
end
function ReqCreateRole_mt:RoleName()
    local o = self.view:Offset(4)
    if o ~= 0 then
        return self.view:String(o + self.view.pos)
    end
end
function ReqCreateRole.Start(builder) builder:StartObject(1) end
function ReqCreateRole.AddRoleName(builder, roleName) builder:PrependUOffsetTRelativeSlot(0, roleName, 0) end
function ReqCreateRole.End(builder) return builder:EndObject() end

return ReqCreateRole -- return the module