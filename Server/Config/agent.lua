local skynet = require "skynet"
local netpack = require "skynet.netpack"
local socket = require "skynet.socket"
local netpack = require "skynet.netpack"

local flatbuffers = require 'FlatBuffers'
local reqlogingame = require 'Login.ReqLoginGame'
local reslogingame = require 'Login.ResLoginGame'
local roleInfoLite = require 'Login.RoleInfoLite'
local proto = require 'protoloader'

local WATCHDOG
local host
local send_request

local CMD = {}
local REQUEST = {}
local client_fd

function REQUEST:get()
	print("get", self.what)
	local r = skynet.call("SIMPLEDB", "lua", "get", self.what)
	return { result = r }
end

function REQUEST:set()
	print("set", self.what, self.value)
	local r = skynet.call("SIMPLEDB", "lua", "set", self.what, self.value)
end

function REQUEST:handshake()
	return { msg = "Welcome to skynet, I will send heartbeat every 5 sec." }
end

function REQUEST:quit()
	skynet.call(WATCHDOG, "lua", "close", client_fd)
end

function REQUEST:ReqLoginGame(content)
	local buf = flatbuffers.binaryArray.New(content)
	local message = reqlogingame.GetRootAsReqLoginGame(buf, 0)

	print('Name ' .. message:Name())
	print('Password ' .. message:Password())

	local builder = flatbuffers.Builder(1024)

	local accountId = builder:CreateString("11111111")

	local roleId1 = builder:CreateString("roleId1")
	local roleName1 = builder:CreateString("roleName1")

	roleInfoLite.Start(builder)
	roleInfoLite.AddRoleId(builder, roleId1)
	roleInfoLite.AddRoleName(builder, roleName1)
	roleInfoLite.AddRoleLevel(builder, 3)
	local role1 = roleInfoLite.End(builder)

	local roleId2 = builder:CreateString("roleId2")
	local roleName2 = builder:CreateString("roleName2")

	roleInfoLite.Start(builder)
	roleInfoLite.AddRoleId(builder, roleId2)
	roleInfoLite.AddRoleName(builder, roleName2)
	roleInfoLite.AddRoleLevel(builder, 3)
	local role2 = roleInfoLite.End(builder)

	local roleId3 = builder:CreateString("roleId3")
	local roleName3 = builder:CreateString("roleName3")

	roleInfoLite.Start(builder)
	roleInfoLite.AddRoleId(builder, roleId3)
	roleInfoLite.AddRoleName(builder, roleName3)
	roleInfoLite.AddRoleLevel(builder, 3)
	local role3 = roleInfoLite.End(builder)

	reslogingame.StartRoleInfoLitesVector(builder, 3)
	builder:PrependUOffsetTRelative(role1)
	builder:PrependUOffsetTRelative(role2)
	builder:PrependUOffsetTRelative(role3)
	local roleInfoLites = builder:EndVector(3)

	reslogingame.Start(builder)
	reslogingame.AddResult(builder, 1)
	reslogingame.AddAccountId(builder, accountId)
	reslogingame.AddRoleInfoLites(builder, roleInfoLites)
	local orc = reslogingame.End(builder)
	
	builder:Finish(orc)

	return { id = proto.getId("ResLoginGame"), data = builder:Output()}
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

local function leftShift(num, shift)
	return math.floor(num * (2 ^ shift));
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
