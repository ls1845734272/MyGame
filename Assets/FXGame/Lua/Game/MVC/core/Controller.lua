local M = class("Controller")

_G.Controller = M

function M:ctor(name)
   self._view = false 
   self.moduleName = name
   self:initServerEvent()
end 

function M:setView( value )
	self._view = value
end

function M:getView()
	-- if not self._view then
	-- 	self._view = self:initView();		
	-- end
	return self._view
end


-- 初始化服务器消息事件侦听
function M:initServerEvent()
	error("Controller:Need override initServerEvent function")
end

-- 初始化视图界面层事件侦听
function M:initViewEvent()
	error("Controller:need override initViewEvent function")
end


return M