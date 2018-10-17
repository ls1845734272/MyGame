local M = class(EventDispatcher,"UIBase")
_G.UIBase = M


M.DEFAULT_VALIDTIME = 90
M.validTime = 90 --有效时间

function M:ctor(params)
	self.isOpen = false
	self.isDestroy = false
	self.destroyTime = 1000000
	self.option = nil
	self.queueList = {}
	self.isPlayQueue = false
end

function M:Open()
	self.isOpen = true
	self.queueList = {}
	self.isPlayQueue = false
	self:dispatchEvent(EventType.WINDOW_OPEN)
end

function M:OpenAnim()
	-- 重载
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

--特效字
function M:playBreathAni(go,toAlpha)
	-- body
	if(go == nil) then
		return
	end
	
	local text = go:GetComponent("Text")
	if(text == nil) then
		log("字体特效，该物体不是文字")
	end
	text.color = Color(text.color.r,text.color.g,text.color.b,1)

	if  Util.GetCurPlatform() then
		text:DOKill(false)
		local tween = text:DOFade(toAlpha or 0.5,1)
		if tween then
			tween:SetLoops (-1, DG.Tweening.LoopType.Yoyo)
		end
	else
		LeanTween.cancel(go.gameObject)
		LeanTween.textAlpha(go:GetComponent("RectTransform"),toAlpha or 0.5,1):setLoopPingPong();
	end
end

--处理ios的兼容问题
function M:stopAni(go)
	-- body
	LeanTween.cancel(go)
	go.transform:DOKill(false)
end

function M:ImageDoFill(img1,endValue1,callBack1) 
	table.insert( self.queueList,{img = img1,endValue = endValue1,callBack = callBack1})
	--if not self.isPlayQueue then 
		self:PlayFill(img1,endValue1,callBack)
	--end 
end 

function M:PlayFill(img,endValue,callBack)
	self.isPlayQueue = true
	if callBack then
		callBack()
	end
	if endValue > img.fillAmount then 
		img:DOFillAmount(endValue,0.1):OnComplete(function()
			table.remove( self.queueList,1)
			if (#self.queueList > 0 ) then 
				self:ImageDoFill(self.queueList[1].img,self.queueList[1].endValue)
			else
				self.isPlayQueue = false
			end 
		end)
	else
		img:DOFillAmount(1,0.05):OnComplete (function()
			img.fillAmount = 0
			img:DOFillAmount(endValue,0.05):OnComplete(function()
				table.remove(self.queueList,1)
				if(#self.queueList>0) then
					self:PlayFill(self.queueList[1].img,self.queueList[1].endValue)
				else
					self.isPlayQueue = false
				end
			end)
		end)
	end 
end 

function M:Close()
	self.isOpen = false
	self:dispatchEvent(EventType.WINDOW_CLOSE) 
end


function M:ClearView()
	
end
