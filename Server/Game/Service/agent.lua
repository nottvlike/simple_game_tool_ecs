local skynet = require "skynet"
local netpack = require "skynet.netpack"
local socket = require "skynet.socket"

local flatbuffers = require 'flatbuffers'

local roleInfo = require 'Protocol.Login.RoleInfo'
local reqRoleInfo = require 'Protocol.Login.ReqRoleInfo'
local resRoleInfo = require 'Protocol.Login.ResRoleInfo'
local reqCreateRole = require 'Protocol.Login.ReqCreateRole'
local resCreateRole = require 'Protocol.Login.ResCreateRole'

local proto = require 'protoloader'

local WATCHDOG
local host
local send_request

local CMD = {}
local REQUEST = {}
local client_fd

function REQUEST:quit()
	skynet.call(WATCHDOG, "lua", "close", client_fd)
end

function REQUEST:ReqRoleInfo(content)
	local buf = flatbuffers.binaryArray.New(content)
	local message = reqRoleInfo.GetRootAsReqRoleInfo(buf, 0)

	local builder = flatbuffers.Builder(1024)

	local roleId = builder:CreateString(message:RoleId())
	local roleName = builder:CreateString("Test1")

	roleInfo.Start(builder)
	roleInfo.AddRoleId(builder, roleId)
	roleInfo.AddRoleName(builder, roleName)
	roleInfo.AddRoleLevel(builder, 3)
	local role = roleInfo.End(builder)

	resRoleInfo.Start(builder)
	resRoleInfo.AddResult(builder, 0)
	resRoleInfo.AddRoleInfo(builder, role)
	local orc = resRoleInfo.End(builder)
	
	builder:Finish(orc)

	return { id = proto.getId("ResRoleInfo"), data = builder:Output()}
end

function REQUEST:ReqCreateRole(content)
	local buf = flatbuffers.binaryArray.New(content)
	local message = reqlogingame.GetRootAsReqLoginGame(buf, 0)

	local builder = flatbuffers.Builder(1024)

	local roleId = builder:CreateString("1")
	local roleName = builder:CreateString(message:RoleName())

	roleInfo.Start(builder)
	roleInfo.AddRoleId(builder, roleId)
	roleInfo.AddRoleName(builder, roleName)
	roleInfo.AddRoleLevel(builder, 3)
	local role = roleInfo.End(builder)

	resCreateRole.Start(builder)
	resCreateRole.AddResult(builder, 0)
	resCreateRole.AddRoleInfo(builder, role)
	local orc = resCreateRole.End(builder)
	
	builder:Finish(orc)

	return { id = proto.getId("ResCreateRole"), data = builder:Output()}
end

local function request(name, args, response)
	local f = assert(REQUEST[name])
	local r = f(REQUEST, args)
	if response then
		return response(r)
	end
end

local function get_response(args)
	local protocolId = args.id
	local bufAsStr = args.data
	return string.pack("<I4i4c2c" .. string.len(bufAsStr), protocolId, string.len(bufAsStr), "00", bufAsStr)
end

local function send_package(pack)
	local package = string.pack(">s2", pack)
	socket.write(client_fd, package)
end

skynet.register_protocol {
	name = "client",
	id = skynet.PTYPE_CLIENT,
	unpack = function (msg, sz)
		if sz > 12 then
			-- 这里更改了 netpack.tostring() 方法，删除了 c 代码中的 skynet_free，不释放 msg
			local bin = netpack.tostring1(msg, sz)
			local protocolId, messageLength = string.unpack("<i4i4", bin)
			local result = bin:sub(11)

			local protocolName = proto.getName(protocolId)
			print('protocolId ' .. protocolId)
			print('messageLength ' .. messageLength)
			print('protocol ' .. protocolName)

			return "REQUEST", protocolName, result, get_response
		end
	end,
	dispatch = function (_, _, type, ...)
		if type == "REQUEST" then
			local ok, result  = pcall(request, ...)
			if ok then
				if result then
					send_package(result)
				end
			else
				skynet.error(result)
			end
		else
			assert(type == "RESPONSE")
			error "This example doesn't support request client"
		end
	end
}

function CMD.start(conf)
	local fd = conf.client
	local gate = conf.gate
	WATCHDOG = conf.watchdog

	client_fd = fd
	skynet.call(gate, "lua", "forward", fd)
end

function CMD.disconnect()
	-- todo: do something before exit
	skynet.exit()
end

skynet.start(function()
	skynet.dispatch("lua", function(_,_, command, ...)
		local f = CMD[command]
		skynet.ret(skynet.pack(f(...)))
	end)
end)
