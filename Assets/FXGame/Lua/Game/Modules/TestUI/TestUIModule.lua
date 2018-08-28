TestUIModule = class(BaseModule,"TestUIModule")

local M = TestUIModule

local tab = {
	
}
function M:ctor( params )
    self.layer = UILayerType.ViewLayer
end


function M:Open()
	self:super("BaseModule","Open")
	UIManager.GetInstance():ShowView("TestUI",{isDontDestroy = true},function(view)
		 	self.testView = view
		 	self.testView:setTestConfig({})
	end)
end

function M:Close()
	self:super("BaseModule","Close")
	UIManager:GetInstance():HideView("TestUI");
end

return M;