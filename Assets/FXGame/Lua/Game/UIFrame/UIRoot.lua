UIRoot = UIRoot or {}
UIRoot._instance = nil 

local M = class("UIRoot")

function UIRoot.GetInstance()
   if UIRoot._instance == nil then 
       UIRoot._instance = M.New()
   end 
   return UIRoot._instance
end 

function M:ctor()

  self.uiRoot = nil 
  self.layerIndex = 0 --层级索引
  self.layers = {};   --层级列表
  self.indexMap = {};


  self:CreateRoot()
end 

function M:AddLyaer(layerName,layerClass)
    layerClass = layerClass or UILayer  --默认层级为UILayer
    local layer = layerClass.New( {index=self.layerIndex,name = layerName} );
    self.indexMap[ layerName ] = self.layerIndex;
    self.layers[ self.layerIndex ] = layer;

    local go = GameObject.New(layerName)
    local rt = go:AddComponent(typeof(RectTransform))
    rt.anchorMin = Vector2.zero
    rt.anchorMax = Vector2.one 
    go.transform:SetParent(self.uiRoot.transform)
    rt.offsetMin = Vector2.zero
    rt.offsetMax = Vector2.zero


    local can = go:AddComponent(typeof(Canvas))
    if  layerName ~= UILayerType.SceneNameLayer 
    and layerName ~= UILayerType.GuideLayer then
        go:AddComponent(typeof(GraphicRaycaster));
    end
    
    can.overrideSorting = true
    can.sortingOrder = self.layerIndex * 500
    layer.startOrder = can.sortingOrder

    go.layer = LayerMask.NameToLayer("UI")
    go.transform.localPosition = Vector2.zero

    layer.gameObject = go;
    layer:init()

    self.layerIndex = self.layerIndex + 1 ;

    return layer;

end 

function M:CreateRoot()
   local go = GameObject.New("UIRoot")
   self.uiRoot = go 
   go.layer = UnityEngine.LayerMask.NameToLayer("UI")
   self.rootRectTs = go:AddComponent(typeof(RectTransform))

    -- 单列  将这个ui放到
    --Object.DontDestroyOnLoad(self.uiRoot)


    local uiCamera = GameObject.New("UICamera")
    local camera = uiCamera:AddComponent(typeof(Camera))
    uiCamera:AddComponent(typeof(UICameraFog))
    --local isMask = 1 << LayerMask.NameToLayer ("UI")
    camera.cullingMask = 32
    camera.clearFlags = CameraClearFlags.Depth
    camera.orthographic = true

    camera.orthographicSize = 360 * (Screen.height / 720)
    camera.nearClipPlane = -8000
    camera.farClipPlane = 8000
    camera.depth = 1

    uiCamera.transform:SetParent(go.transform)
    uiCamera.transform.localPosition = Vector3.one

    self.uiCamera = uiCamera
    self.uiCamera_cam = camera

    local canvas = go:AddComponent(typeof(Canvas))
    canvas.worldCamera = camera
    canvas.renderMode = RenderMode.ScreenSpaceOverlay
    
    local timer = Timer.New(function()
       canvas.renderMode = RenderMode.ScreenSpaceOverlay
    end,0,1)

    local cs = go:AddComponent(typeof(CanvasScaler))
    cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize
    cs.referenceResolution = Vector2.New(1280,720)
    cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight
    cs.matchWidthOrHeight = 1 -- 高对齐

    for _,v in ipairs(UILayerInfos) do 
        self:AddLyaer(v.name,v.class)
    end 
    
    local esObj = GameObject.Find("EventSystem")
    if esObj ~= nil then 
        GameObject.Destroy(esObj)
    end 

    local eventObj = GameObject.New("EventSystem")
    eventObj.layer = LayerMask.NameToLayer("UI")
    eventObj.transform:SetParent(go.transform)
    eventObj:AddComponent(typeof(EventSystem))

    eventObj:AddComponent(typeof(UnityEngine.EventSystems.StandaloneInputModule))
end 


-- 屏幕坐标转UI坐动态改变屏幕比例
function M:ScreenToUIPosition(x,y,z)
    return self.uiCamera_cam:ScreenToWorldPoint(Vector3(x,y,z))
end

-- ui转到root位置
function M:ChildToRootPos(uiPath)
    if not uiPath or uiPath == '' then return end
    if not self.rootRectTs then
        self.rootRectTs = self.uiRoot:GetComponent(typeof(UnityEngine.RectTransform))
    end

    local ui = self.rootRectTs:Find(uiPath)
    local rectTs = ui and ui:GetComponent(typeof(UnityEngine.RectTransform)) or nil
    if not rectTs then return end

    local cenX,cenY = rectTs.rect.x + rectTs.rect.width * 0.5,rectTs.rect.y + rectTs.rect.height * 0.5
    local worldPos =  rectTs:TransformPoint(Vector3(cenX,cenY,0))
    return self.rootRectTs:InverseTransformPoint(worldPos)
end

--获得层级
function M:getLayer(name)
    if name==nil or name=="" then return end 
    local index = self.indexMap[ name ]
    return self.layers[ index ]
end

return UIRoot