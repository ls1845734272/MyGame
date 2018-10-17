
UIManager = UIManager or {}
UIManager._instance = nil 

local M = class("UIManager")

function UIManager.GetInstance()
     if UIManager._instance == nil then 
         UIManager._instance = M.New()
     end 
     return UIManager._instance
end 

function M:ctor()
   self:init()
end 

function M:init()

    self.UIRoot = require("Game/UIFrame/UIRoot").GetInstance()
    --所有已经加载的Views 缓存
    self.allViews = {}
    -- 异步过程中关闭面板
    self.asyncHideViews = {}
    --当前正在显示的Views
    self.currentViews = {}
    self._delayShowViews = {}
    self._curShownViews = {}
    self.downLoadedMap = {}
    self.loadDict = {}

    self.recycleTimerDic = {} -- 回收跟踪
    self.viewLayerStates = {} -- viewLayer中面板开/关状态(用于判断摄像机开/关)
    self.isPreShowing = {}
end 

function M:initView(view,viewData,viewConfig)
   if (not view) or (not view:GetComponent("LuaBehaviour")) then 
        print("找不到脚本：",viewConfig.viewId)
        return nil
   end 
    local lb = view:GetComponent('LuaBehaviour')
    local instance = view:GetComponent('LuaBehaviour'):GetInstance()
    if instance then
        instance.viewData = viewData
        instance.cfg = viewConfig
        instance.viewId = viewConfig.viewId
        instance.layer = viewConfig.layer
    end    
    return instance
end 

--打开窗口
function M:ShowView(viewId,param,callBack,isPreload)
    local param = param or {}
    local fnCallback = param.fnCallback
    local viewData = param.viewData
    local isAsync = param.isAsync
    local sync = (viewData and type( viewData ) == 'table' and viewData.sync) and true or param.sync
    local viewConfig = UIViewID[viewId]
    -- print("ShowView ",viewId)
    if not viewConfig then
        log(viewId.." 未配置，请前往 ui_def.lua 进行配置")
        return
    end

    --打开的时候不传参isDontDestroy 默认按照UIViewID配置的isDontDestroy
    local isDontDestroy = viewConfig.isDontDestroy
    local layer = (param.layer or viewConfig.layer)
    --默认
    isDontDestroy = isDontDestroy or false
    layer = layer or "ViewLayerDefault"


    viewConfig.layer = layer 
    viewConfig.viewId = viewId
    viewConfig.delayProcess = viewConfig.delayProcess or 0  --菊花
    local recycleTime = viewConfig.recycleTime or 10   --默认10秒回收

    -- 内存1g的手机
    if FXGame.AppConst.forceLowQuality then
        recycleTime = 2
        isDontDestroy = false
    end

    self.isPreShowing[viewId] = true
    self.asyncHideViews[viewId] = nil


    if table.containKey(self.allViews,viewId) then 
        
     --已经存在，预制   
        local _view = self.allViews[viewId].obj
        self.allViews[viewId].isHideing = false
        self.allViews[viewId].layer = layer
        self.allViews[viewId].isDontDestroy = isDontDestroy
        self.allViews[viewId].recycleTime = recycleTime
        _view.gameObject:SetActive(true)

        self:addToLayer(_view,layer,viewId)
        local instance = self:initView(_view,viewData,viewConfig)

        xpcall( function( )
            instance:Open()
            instance.gameObject.layer = LayerMask.NameToLayer("UI")
        end,function()
            if  UnityEngine.Application.isEditor then
              log("error:"..debug.traceback())
            end
        end)

        if not isPreload  then
            instance:OpenAnim()
        end

        -- 回收跟踪
        if recycleTime > 0 then
            if self.recycleTimerDic[viewId] and self.recycleTimerDic[viewId].running then
                self.recycleTimerDic[viewId]:Stop()
            end
            self.recycleTimerDic[viewId] = Timer.New()
        end

        if callBack then
            callBack(instance)
        end
    else

        if not self.loadDict[viewId] then 
            self.loadDict[viewId] = {viewData = viewData,cfg = viewConfig}
            self.loadDict[viewId].syncLoad = sync 

            self:_createView(viewId,self.loadDict[viewId],function(view,instance)

               if view == nil then 
                    self.loadDict[viewId] = nil
                    self.allViews[viewId] = nil
                    self.viewLayerStates = {}


                    print("该界面初始化完成，请重新登录")
                    return 
               end 

                --instance:PreView()  --不知
                self:addToLayer(view,layer,viewId)

                local oldLocalPos = view.transform.localPosition
                UIUtils:setVisible(view,false)

                CallDelay:nextCall(function()
                    local function execute()
                        instance.viewData = self.loadDict[viewId].viewData
                        instance:Open()
                        instance.gameObject.layer = LayerMask.NameToLayer("UI")
                    end
                    
                    if UnityEngine.Application.isEditor then
                        execute()
                    end

                    UIUtils:setVisible(view,true,oldLocalPos)  

                    self.allViews[viewId] = {obj = view, 
                                    isDontDestroy = isDontDestroy,
                                    layer = layer,
                                    recycleTime = recycleTime,
                                    isHideing = false}

                    if not isPreload  then
                        instance:OpenAnim()
                    end

                    -- 回收跟踪
                    if recycleTime > 0 then
                        if self.recycleTimerDic[viewId] and self.recycleTimerDic[viewId].running then
                            self.recycleTimerDic[viewId]:Stop()
                        end
                        self.recycleTimerDic[viewId] = Timer.New()
                    end

                    if callBack then
                        callBack(instance)
                    end
                end)
            end)
        else
            self.loadDict[viewId] = {viewData=viewData,cfg=viewConfig,}
            self.loadDict[viewId].syncLoad = sync
        end 
    end 
end 

function M:_createView(viewId,loadInfo,onComplete)

    if not loadInfo or not onComplete then 
        return 
    end

    local cfg = loadInfo.cfg
    local viewId = cfg.viewId
    local viewData = loadInfo.viewData
    local syncLoad = loadInfo.syncLoad

    local path = "FXGame/Prefabs/UI/"..cfg.path..".prefab"    

    if UIViewPath[viewId] then -- 加载lua文件
       require(UIViewPath[viewId])
    end

    local instance = nil 

    UIUtils:LoadUI(path,function(go)
    	if not go then 
           print("预制不存在:",viewId,path)
           self.loadDict[viewId] = nil
           return
    	end 

         instance = self:initView(go,viewData,cfg)

         if go then
             go.name = viewId
             onComplete(go,instance)
         else
       	     onComplete(nil)
         end 
    end)
end 

function M:GetView(viewId)
    -- body
    local _view = nil
    if(table.containKey(self.allViews,viewId)) then
        _view = self.allViews[viewId].obj 
    end
    if _view then
        local instance = _view:GetComponent("LuaBehaviour"):GetInstance()
        return instance
    end
end

--view 对象预制体，layer 层级  ，viewId 注册的名字
function M:addToLayer(view,layer,viewId)
    -- body
    if view == nil or view.gameObject == nil then
        return 
    end
    local layerObj = self.UIRoot:getLayer(layer)
    assert(layerObj,"layer is nil")
    
    if layer == "ViewLayer" or layer == "ViewLayerDefault" then
        layerObj:addChild(view,viewId)
    else
        layerObj:addChild(view)
    end
end


function M:HideView(viewId,isForceDestroy,isPreload)
    local struct = self.allViews[viewId]
    if struct == nil then
          return 
    end

    self.isPreShowing[viewId] = nil

    self.allViews[viewId].isHideing = true
    self.loadDict[viewId] = false

    local instance = struct.obj:GetComponent("LuaBehaviour"):GetInstance()
    xpcall(function()
            instance:Close()
        end,function()
        if  UnityEngine.Application.isEditor then
                log("error:"..debug.traceback())
            end
        end)

    local layer = self.UIRoot:getLayer(struct.layer)

    print(isForceDestroy,"isForceDestroy")
    
    if isForceDestroy then
        if self.recycleTimerDic[viewId] then
            self.recycleTimerDic[viewId]:Stop()
            self.recycleTimerDic[viewId] = nil
        end
        layer:removeChild(struct.obj,false,viewId,struct.layer,true)
        self.allViews[viewId] = nil
        --instance:Recycle()--uibase里面的方法，内存回收
    else
        print(struct.isDontDestroy,"struct.isDontDestroy")
        if not struct.isDontDestroy then
            print(struct.recycleTime,"struct.recycleTime")
            if struct.recycleTime > 0 then
                if self.recycleTimerDic[viewId] then
                    if self.recycleTimerDic[viewId].running then
                        self.recycleTimerDic[viewId]:Stop()
                    end
                    self.recycleTimerDic[viewId]:Reset(function()
 
                     if not self:isShowing(viewId) then
                         layer:removeChild(struct.obj,false,viewId,struct.layer,true)
                         self.allViews[viewId] = nil
                     end
 
                     end,struct.recycleTime,1)
                     self.recycleTimerDic[viewId]:Start()
                 end
                 layer:removeChild(struct.obj,true,viewId,struct.layer)
            else
                layer:removeChild(struct.obj,false,viewId,struct.layer,true)
                self.allViews[viewId] = nil
            end
        else
            layer:removeChild(struct.obj,true,viewId,struct.layer)
        end
    end 
    --layer:removeChild(struct.obj,false,viewId,struct.layer,true)

    layer:updateAllSortingOrder()

    --self.allViews[viewId] = nil
    --UIUtils:setVisible(view,false)
end 


function M:isShowing(viewId)
    for id,struct in pairs(self.allViews) do
        if viewId == id then
            if (not struct.isHideing) and struct.obj.activeSelf and self.isPreShowing[viewId] then
                return true
            end
        end
    end
    return false
end

--中间的飘字  itemBaseId 这是物品code；checkClick 这是是否需要cd
function UIManager.ShowAlert(text,color,itemBaseId,checkClick)
    color = color or ColorType.white.hex
    UIManager.GetInstance():ShowView("CommonAlert",{viewData={content=text,color=color,baseId=itemBaseId,checkClick=checkClick}})
end

return UIManager