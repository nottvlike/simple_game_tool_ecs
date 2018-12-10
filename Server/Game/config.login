include "config.path"

project = "./Game/"
luaservice = project .. "Service/Login/?.lua;" .. luaservice
lua_path = project .. "?.lua;" .. project .."Logic/?.lua;" .. project.."LuaLib/?.lua;" .. lua_path

-- preload = "./examples/preload.lua"	-- run preload.lua before every lua service run
thread = 8
logger = nil
logpath = "."
harbor = 1
address = "127.0.0.1:2527"
master = "127.0.0.1:2013"
start = "login"	-- main script
bootstrap = "snlua bootstrap"	-- The service for bootstrap
-- standalone = "0.0.0.0:2013"
-- snax_interface_g = "snax_g"
cpath = root.."cservice/?.so"
-- daemon = "./skynet.pid"
