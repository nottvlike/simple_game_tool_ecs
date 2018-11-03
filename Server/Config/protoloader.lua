local proto = {}

local protocolsNames = {}
local protocolIds = {}
local function load()
	local protocols = require 'Protocols'
	
	for i,v in pairs(protocols) do
		protocolsNames[i] = v
	end

	for i,v in pairs(protocols) do
		protocolIds[v] = i
	end
end

function proto.getName(protocolId)
	return protocolIds[protocolId] 
end

function proto.getId(protocolName)
	return protocolsNames[protocolName] 
end

load()

return proto