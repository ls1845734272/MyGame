ModuleManager = ModuleManager or {}

local M = ModuleManager


function M:sendModule(moduleType,viewData)
    local moduleView = self:getModule(moduleType)
    moduleView.viewData = viewData
    if moduleView.isOpen then
        moduleView:Close()
    else
        moduleView:Open()
    end
    return moduleView
end

function M:openModule(moduleType,viewData)
   local moduleView = self:getModule(moduleType)
    moduleView.viewData = viewData
    moduleView:Open()    
    return moduleView
end 

function M:getModule(moduleType)
   local moduleView = GameModule[moduleType] 
   if not moduleView then
      local cfg = moduleConfig[moduleType]
      GameModule[moduleType] = require(cfg.module).New(cfg.module)
      moduleView = GameModule[moduleType]
   end 
   moduleView.viewData = viewData

   if  controller and not controller:getView() then
        controller:setView(moduleView);
        controller:initView()
        controller:initViewEvent();--controller里面的事件监听
   end 

   return moduleView
end 