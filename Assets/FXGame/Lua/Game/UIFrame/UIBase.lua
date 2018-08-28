local M = class(EventDispatcher,"UIBase")
_G.UIBase = M


M.DEFAULT_VALIDTIME = 90
M.validTime = 90 --有效时间

function M:ctor(params)
	self.isOpen = false
	self.isDestroy = false
	self.destroyTime = 1000000
	self.option = nil
end

function M:isActive()
	local ret = self.gameObject ~=nil and self.gameObject.activeSelf
	return ret
end

function M:PlayAnimOpen(isNotMask,isRaycastMask,maskPec,callBackFunc)
    local group = self.gameObject:GetComponent(typeof(UnityEngine.CanvasGroup))
	if group == nil then
		group = self.gameObject:AddComponent(typeof(UnityEngine.CanvasGroup))
	end
	group.alpha = 1
	local targetScale = Util.getAdpTargetScale()

    self.transform.localScale = Vector3( 0.7 * targetScale,0.7 * targetScale,1)

	LeanTween.cancel(self.gameObject)
	-- setOnUpdate（）
	LeanTween.scale(self.gameObject:GetComponent("RectTransform"),Vector3(targetScale,targetScale,1),0.5):setEase(LeanTweenType.easeOutBack)
	:setOnComplete(System.Action(function()
		if callBackFunc then
			callBackFunc()
		end
	end))

	self:AddMask(isRaycastMask,isNotMask,maskPec)
end 

function M:PlayAnimClose(cbFunc)
	-- body
	xpcall( function( )
		local targetScale = Util.getAdpTargetScale()

		self.transform.localScale = Vector3(targetScale,targetScale,1)
		local group = self.gameObject:GetComponent(typeof(UnityEngine.CanvasGroup))
		if group == nil then
			group = self.gameObject:AddComponent(typeof(UnityEngine.CanvasGroup))
		end
		group.alpha = 1

		
		LeanTween.cancel(self.gameObject)
		LeanTween.alphaCanvas(group,0,1):setEase(LeanTweenType.easeOutCirc)
		LeanTween.scale(self.gameObject:GetComponent("RectTransform"),Vector3(0,0,0),0.3)
		:setEase(LeanTweenType.easeInBack)
		:setOnComplete(System.Action(cbFunc))

		self:RemoveMask()
	end,function()
		if UnityEngine.Application.isEditor then
           log("error:"..debug.traceback())
           cbFunc()
        end	
	end)
end



--添加蒙黑
function M:AddMask(isRaycastMask,isNotMask,maskPec)
	if self._maskLayer then
		Object.Destroy(self._maskLayer)
		self._maskLayer = nil
	end

	self._maskLayer = GameObject.New("Mask")
	self._maskLayer.layer = LayerMask.NameToLayer("UI")
	local img = self._maskLayer:AddComponent(typeof(Image))
	img.color = Color(0,0,0,0)
	if isRaycastMask ~= nil then
		img.raycastTarget = isRaycastMask
	end

	local rt = self._maskLayer:GetComponent(typeof(RectTransform))
	rt.anchorMin = Vector2.zero
	rt.anchorMax = Vector2.one 
	self._maskLayer.transform:SetParent(self.transform.parent)
	rt.offsetMin = Vector2.zero
	rt.offsetMax = Vector2.zero

	self._maskLayer.transform:SetSiblingIndex(self.transform:GetSiblingIndex())
	self._maskLayer.transform.localPosition = Vector3.zero
	self._maskLayer.transform.localScale = Vector3(1,1,1)

	-- local selfSo = self.gameObject:GetComponent(typeof(SortingOrder))
	-- if selfSo then
	-- 	local maskSo = self._maskLayer:AddComponent(typeof(SortingOrder))
	-- 	maskSo.isUI = true
	-- 	maskSo.sortingOrder = selfSo.sortingOrder -1
	-- end

	if not isNotMask then
		LeanTween.alpha(rt,maskPec or 0.6,0.3):setEase(LeanTweenType.easeOutQuad)
	end
end


--移除蒙黑
function M:RemoveMask()
	if self._maskLayer then
		local rt = self._maskLayer:GetComponent(typeof(RectTransform))
		LeanTween.alpha(rt,0,0.3):setEase(LeanTweenType.easeInQuad):setOnComplete(System.Action(function()
			Object.Destroy(self._maskLayer)
			self._maskLayer = nil
		end))
	end
end

function M:CheckRemoveMask()
	if self._maskLayer then
		Object.Destroy(self._maskLayer)
		self._maskLayer = nil
	end
end

function M:HideMaskLayer()
	if self._maskLayer then
		self._maskLayer:SetActive(false)
	end
end

function M:ShowMaskLayer()
	if self._maskLayer then
		self._maskLayer:SetActive(true)
	end
end

--渐入
function M:PlayFadeIn(target,func,time)
   if type(target) == "number" then return end 

   if target ~= nil then 
       local group = target.gameObject:GetComponent(typeof(UnityEngine.CanvasGroup))
       if group == nil then 
          group = target.gameObject:AddComponent(typeof(UnityEngine.CanvasGroup))
       end 
       group.alpha = 0
       LeanTween.cancel(target.gameObject)
       LeanTween.alphaCanvas(group,1,time or 0.2):setOnComplete(System.Action(function()
             if func then 
                func()
             end 
       	end))
   else
   		local group = self.gameObject:GetComponent(typeof(UnityEngine.CanvasGroup))
		if group == nil then
			group = self.gameObject:AddComponent(typeof(UnityEngine.CanvasGroup))
		end
		group.alpha = 0
		LeanTween.cancel(self.gameObject)
		LeanTween.alphaCanvas(group,1,time or 0.2):setOnComplete(System.Action(function()
			if func then
				func()
			end
		end))
   end 
end 


--渐出
function M:PlayFadeOut(cbFunc,target,time,res)
	if type(target) == "number" then return end

	if target ~= nil then
		local group = target.gameObject:GetComponent(typeof(UnityEngine.CanvasGroup))
		if group == nil then
			group = target.gameObject:AddComponent(typeof(UnityEngine.CanvasGroup))
		end
		group.alpha = 1
		LeanTween.cancel(target.gameObject)
		LeanTween.alphaCanvas(group,0,time or 0.2):setOnComplete(System.Action(function()
			if res then
				group.alpha = 1
			end
			if cbFunc then
				cbFunc()
			end
		end))
	else
		local group = self.gameObject:GetComponent(typeof(UnityEngine.CanvasGroup))
		if group == nil then
			group = self.gameObject:AddComponent(typeof(UnityEngine.CanvasGroup))
		end
		group.alpha = 1
		LeanTween.cancel(self.gameObject)
		LeanTween.alphaCanvas(group,0,time or 0.2):setOnComplete(System.Action(function()
			if cbFunc then
				cbFunc()
			end
		end))
	end
end







--------------600--------------------
--设置小奖的默认位置
function Maze:DefualtSmallReward(idx)
    local commItem = self.samllItemList[idx].gameObject
    if commitem == nil then return end 

    local curpostion = Vector3(0,0,0)
    if self.gridPositionList[idx] then 
        curpostion = self.gridPositionList[idx]
    else
        curpostion = self:SetPosition(idx)
    end 

    commItem.transform.localScale = Vector3(0.6,0.6,0.6);
    commItem.transform.localPosition = curpostion

    commItem:SetActive(false)
    if i <= self.info.num then
        commItem:SetActive(true)
    end 
end 

function Maze:PlaySmallItemAnim(obj)
    local group = self.inje_Image.gameObject:GetComponent(typeof(UnityEngine.CanvasGroup))
    group.alpha = 1

    LeanTween.cancel(self.inje_Image.gameObject)
    LeanTween.moveLocalY(self.inje_Image.gameObject,100,10)  -- 位置
    LeanTween.scale(self.inje_Image.gameObject, Vector3(2,2,2),4) -- 缩放
    
    LeanTween.value(self.inje_Image.gameObject,100,0,4)
    :setEase(LeanTweenType.easeOutQuad)
    :setOnUpdate(System.Action_float(function(value)
        self.inje_Image.transform:GetComponent("RectTransform").sizeDelta = Vector2(value,value)
    end))

    LeanTween.alphaCanvas(group,0,5) -- 渐隐
end 


