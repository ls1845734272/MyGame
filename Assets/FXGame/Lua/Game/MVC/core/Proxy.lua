local M = class("Proxy")
_G.Proxy = M

function M:ctor(name)
	self.moduleName = name;
	--self.net = Message.Net;
end

return M