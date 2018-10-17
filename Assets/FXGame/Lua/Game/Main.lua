
local GameObject = UnityEngine.GameObject;

-- --所有的模块导入
-- local breakInfoFun = require("Game.LuaDebugjit")(ip and ip or "localhost",7003)
--     --添加断点监听函数
-- local time = Timer.New(breakInfoFun,0.5,-1,1)
-- time:Start()

require("Game/Defines/define")

--主入口函数。从这里开始lua逻辑
function main()	
	print("----------------------------------------------------------")		
	--local mm = UnityEngine.GameObject.New("UIRoot")

    -- local path = "FXGame/Prefabs/UI/Module/Test/TestUI.prefab"
    -- UIUtils:LoadUI(path,function(go)
		  --  if (not go) or (not go:GetComponent("LuaBehaviour")) then 
		  --       print("找不到脚本：")
		  --       return nil
		  --   else
		  --   	print("iiiiiiiiiiiiiiiiiiii")
		  --  end 
    -- 	end )

    UIManager.GetInstance():ShowView("TestUI")
end

--场景切换通知
-- function OnLevelWasLoaded(level)
-- 	collectgarbage("collect")
-- 	Time.timeSinceLevelLoad = 0
-- end
