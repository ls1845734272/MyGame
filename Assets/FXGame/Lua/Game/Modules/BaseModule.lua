local M = class(UIBase,"BaseModule")
_G.BaseModule = M

function M:ctor( params )
	self.moduleName = "";
	self.isOpen = false
end

function M:Open()
	self.isOpen = true
end

function M:Close()
   self.isOpen = false 
end

return M