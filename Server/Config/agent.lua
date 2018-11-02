local skynet = require "skynet"
local netpack = require "skynet.netpack"
local socket = require "skynet.socket"
local netpack = require "skynet.netpack"

local flatbuffers = require 'FlatBuffers'
local reqlogingame = require 'ReqLoginGame'
local reslogingame = require 'ResLoginGame'
local roleInfoLite = require 'RoleInfoLite'
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
	print('Channel ' .. message:Channel())
	print('SubChannel ' .. message:SubChannel())
	print('ChannelName ' .. message:ChannelName())
end

local function request(name, args, response)
	local f = assert(REQUEST[name])
	local r = f(REQUEST, args)
	if response then
		return response(r)
	end
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

			return "REQUEST", protocolName, result, nil
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
	-- slot 1,2 set at main.lua
	skynet.fork(function()
		while true do
			send_package("heartbeat")
			skynet.sleep(500)
		end
	end)

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
