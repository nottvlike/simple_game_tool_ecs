local skynet = require "skynet"
local socket = require "skynet.socket"

skynet.start(function()
	local client = {}
	for i= 1, 20 do
		client[i] = skynet.newservice("httpclient")
	end
	local balance = 1
	local id = socket.listen("0.0.0.0", 8001)
	skynet.error("Listen web port 8001")
	socket.start(id , function(id, addr)
		skynet.error(string.format("%s connected, pass it to client :%08x", addr, client[balance]))
		skynet.send(client[balance], "lua", id)
		balance = balance + 1
		if balance > #client then
			balance = 1
		end
	end)
end)