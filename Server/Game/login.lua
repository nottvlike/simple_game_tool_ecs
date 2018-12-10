local skynet = require "skynet"

skynet.start(function()
	skynet.error("Server start")

	skynet.newservice("httplistener")

	skynet.exit()
end)
