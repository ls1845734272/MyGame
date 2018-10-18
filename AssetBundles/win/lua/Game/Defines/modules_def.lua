-- 模块
--   module1
--    |_ _ _ Controller
--    |_ _ _ Module
--    |_ _ _ Proxy
--    |_ _ _ view
-- 			   | _ _ _  View1
--             | _ _ _  View2

GameControllers = GameControllers or {} --控制器集

GameProxy  = GameProxy  or {} --代理集

GameModule = GameModule or {} --模块集

Cache = Cache or {} --缓存数据集

print("Module_def")
GameControllers[ModuleType.TestUI] = require("Game/Modules/TestUI/TestUIController" ).New(ModuleType.TestUI)
GameProxy[ModuleType.TestUI] = require( "Game/Modules/TestUI/TestUIProxy" ).New(ModuleType.TestUI)
