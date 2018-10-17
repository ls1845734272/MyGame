local M = class(Controller,"TestUIController")


function M:ctor()
	require("Game/Modules/TestUI/TestUIData")
end

function M:initServerEvent()
	--NetDispatcher.addEventListener(NetEventType.TEST_GETCONFIG_RES,self.onTestConfigResHandler,self);
    Dispatcher.addEventListener(EventType.FIEST_EVENT,self.FirstUpdate,self);
end


function M:initViewEvent()
	--Dispatcher.addEventListener(EventType.TEST_GETCONFIG,self.onGetConfigHandler,self);
end

function M:FirstUpdate( ... )
	print("点击了当前的button")
end

return M