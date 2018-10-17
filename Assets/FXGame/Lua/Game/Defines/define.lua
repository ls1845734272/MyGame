-- unit基础类型全局定义
require("Game/Defines/type_def")

-- 基础工具
require("Game/Common/Class")
require("Game/Common/Util")
require("Game/Common/SystemClass")
require("Game/Util/GlobalUtil")


require("Game/Util/HtmlTextUtil")
--延迟处理
require('Game/Util/CallDelay')

------------------------------------------
--事件派发模块
require("Game/MVC/core/EventDispatcher")
require("Game/MVC/core/Dispatcher")

--------------------------------------
--颜色
require( "Game/Defines/color_def" ) 

--普通事件定义
require( "Game/Defines/EventType" )
----------------------------------------------
--界面
require( "Game/Defines/ui_def" )

-----------------------------------------------------------
-- 管理器及核心模块初载入
require("Game/Defines/manager_def")

-- 游戏模块载入   导入GameController ,GameProxy , GameModule , Cache 四个tabel
require("Game/Defines/modules_def" )