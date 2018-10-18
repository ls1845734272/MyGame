TestUIModule = class(BaseModule,"TestUIModule")

local M = TestUIModule

local tab = {
	
}
function M:ctor( params )
    self.layer = UILayerType.ViewLayer
end


function M:Open()
	print("是否执行了这里的逻辑open")
	self:super("BaseModule","Open")
	UIManager.GetInstance():ShowView("TestUI",{isDontDestroy = true},function(view)
		 	self.testView = view
	end)
end

function M:Close()
	print("关闭了当前的测试界面")
	UIManager:GetInstance():HideView("TestUI");
end

return M;