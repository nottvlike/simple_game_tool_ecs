local skynet = require "skynet"
local socket = require "skynet.socket"
local httpd = require "http.httpd"
local sockethelper = require "http.sockethelper"
local urllib = require "http.url"
local table = table
local string = string

local CMD = 
{
	"/login" = function(id, params)
		skynet.error(string.format("%s login success!", params["user"]))
		response(id, 200, [[
				{
					"result": 0,
					"accountId": "111111111111111",
					"roleInfoLiteList": [{
						"roleId": "1",
						"roleName": "Test111",
						"roleLevel": 1,
						"serverId": 1
					}, {
						"roleId": "2",
						"roleName": "Test222",
						"roleLevel": 2,
						"serverId": 2
					}, {
						"roleId": "3",
						"roleName": "Test333",
						"roleLevel": 3,
						"serverId": 3
					}]
				}
			]])
	end,
	"/register" = function(id, params)
		skynet.error(string.format("%s register success!", params["user"]))
		response(id, 200, [[
				{
					"result": 0,
					"accountId": "111111111111111",
					"roleInfoLiteList": []
				}
			]])
	end,
	"/serverInfo" = function(id, params)
		skynet.error(string.format("%s get server info success!", id))
		response(id, 200, [[
				{
					"result": 0,
					"serverInfoList": [{
						"serverId": "1",
						"serverName": "Test1",
						"serverAddress": "127.0.0.1",
						"serverPort": 8888
					}, {
						"serverId": "2",
						"serverName": "Test2",
						"serverAddress": "127.0.0.1",
						"serverPort": 8888
					}, {
						"serverId": "3",
						"serverName": "Test3",
						"serverAddress": "127.0.0.1",
						"serverPort": 8888
					}]
				}
			]])
	end
}

local function response(id, ...)
	local ok, err = httpd.write_response(sockethelper.writefunc(id), ...)
	if not ok then
		-- if err == sockethelper.socket_error , that means socket closed.
		skynet.error(string.format("fd = %d, %s", id, err))
	end
end

skynet.start(function()
	skynet.dispatch("lua", function (_,_,id)
		socket.start(id)
		-- limit request body size to 8192 (you can pass nil to unlimit)
		local code, url, method, header, body = httpd.read_request(sockethelper.readfunc(id), 8192)
		if code then
			if code ~= 200 then
				response(id, code)
			else
				local tmp = {}
				if header.host then
					table.insert(tmp, string.format("host: %s", header.host))
				end

				local path, query = urllib.parse(url)
				local params = {}				if query then
					params = urllib.parse_query(query)
				end

				if CMD[path] then
					CMD[path](params)
				else
					response(id, code, "{}")
				end
			end
		else
			if url == sockethelper.socket_error then
				skynet.error("socket closed")
			else
				skynet.error(url)
			end
		end
		socket.close(id)
	end)
end)