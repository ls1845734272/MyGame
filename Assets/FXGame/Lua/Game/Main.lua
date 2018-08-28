
local GameObject = UnityEngine.GameObject;


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
