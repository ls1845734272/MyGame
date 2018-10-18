-- 提示
CommonAlert = class(UIBase,"CommonAlert")
CommonAlert.inje_AlertItem = 0

function CommonAlert:Awake()
	
	self._alertItemPath = "Prefabs/UI/Common/AlertItem.prefab"
	self.pop_msgs = {} --数据队列
	self.item_list = {}
	self.cache_list = {}
	self._isAnimLock = false

	self.speed = 130
	self.fadeOutSpeed = 0.5
	self.pos_list = {}
	for i=0,8 do
		self.pos_list[i] = Util.Find(self.gameObject,"p"..i)
	end
	self._isCanPushMsg = true

	self._timerMsgLock = Timer.New()
	self._autoHideTimer = Timer.New()
end

function CommonAlert:Open()
	if not self.handle then
		self.handle = UpdateBeat:CreateListener(self.Update, self)
	end

	UpdateBeat:RemoveListener(self.handle)
	UpdateBeat:AddListener(self.handle)

	if self.viewData.checkClick == nil then --默认开启CD检测
		self.viewData.checkClick = true
	end

	if self.viewData.checkClick and (self._isCanPushMsg == false) then
		return
	end

	table.insert(self.pop_msgs,self.viewData)
	
	self:checkShow()

	if self.viewData.checkClick then
		self._isCanPushMsg = false
		self._timerMsgLock:Reset(function()
			self._isCanPushMsg = true
		end,0.2,1)
		self._timerMsgLock:Start()
	end
end 

function CommonAlert:Start()

	self.transform.localScale = Vector3.one * Util.getAdpTargetScale()
	self:checkShow()
end

function CommonAlert:checkShow()
	if #self.pop_msgs <= 0 then
		return
	end

	if self._isAnimLock then return end

	local msg = self.pop_msgs[1]

	local item = self:getCacheItem(self._alertItemPath)
	if not item then return end
	if item:Equals(nil) then return end

	item:GetComponent("CanvasGroup").alpha = 0
	item.transform:SetParent(self.transform)
	item.transform.position = self.pos_list[0].transform.position
	item.transform.localScale = Vector3(1,1,1)
	self:showItem(item,msg)--设置 显示
	table.remove(self.pop_msgs,1)

	local struct = {item=item}
	table.insert(self.item_list,struct)
	self._isAnimLock = true

	UpdateBeat:RemoveListener(self.handle)
	UpdateBeat:AddListener(self.handle)

	local t1 = 0.2
	local t2 = 1.8
	LeanTween.cancel(item)
	LeanTween.alphaCanvas(item:GetComponent("CanvasGroup"),1,t1):setOnComplete(System.Action(function()
		LeanTween.delayedCall(item,t2,System.Action(function()
			LeanTween.alphaCanvas(item:GetComponent("CanvasGroup"),0,self.fadeOutSpeed):setOnComplete(System.Action(function()
				self:RecycleItem(item)
			end))
		end))
	end))
	
	--如果不加这个判断，self._autoHideTimer:Stop() 方法里面的self.handle是一个空，会报错
    if (self._autoHideTimer.running) then 
		self._autoHideTimer:Stop()
	end
	self._autoHideTimer:Reset(function()
		UpdateBeat:RemoveListener(self.handle)
		UIManager.GetInstance():HideView("CommonAlert")
	end,t1 + t2 + self.fadeOutSpeed,1)
	self._autoHideTimer:Start()

end 

function CommonAlert:Close()
	UpdateBeat:RemoveListener(self.handle)
	self.item_list = {}
	self.pop_msgs = {}
	self._isCanPushMsg = true
	self._isAnimLock = false
end

function CommonAlert:Update()
	self._isAnimLock = false
	local resoule = GlobalUtil.getResolution()

	if #self.item_list >= 3 then
		self.speed = 190 * resoule
	else
		self.speed = 130 * resoule
	end

	local appendY = 0
	local lastItem = nil
	--2
	for i=1,#self.item_list do
		local idx = #self.item_list - (i - 1) --倒叙
		local struct = self.item_list[idx]
		if struct then
			local item = struct.item
			if item and item.gameObject then
				if idx == #self.item_list then

					if item.transform.position.y <= self.pos_list[1].transform.position.y then
						self._isAnimLock = true
					end

					if item.transform.position.y <= self.pos_list[2].transform.position.y then
						
						appendY = self.speed * Time.deltaTime
						item.transform.position = item.transform.position + Vector3(0,appendY,0)
					end
				end

				local dis = 0
				if self.lastItem then
					dis = item.transform.position.y - self.lastItem.transform.position.y
				end

				if idx ~= #self.item_list and appendY > 0 and dis <= 48 * resoule then
					item.transform.position = item.transform.position + Vector3(0,appendY,0)
				end

				self.lastItem = item

				if item.transform.position.y >= self.pos_list[3].transform.position.y + 10 then --超出要隐藏
					-- Object.Destroy(item)
					-- CLuaUtil.PushPool(item,self._alertItemPath)
					self:RecycleItem(item)
					table.remove(self.item_list,1)
					self.lastItem = nil
				end
			end
		end
	end
	if self._isAnimLock == false then
		self:checkShow()
	end
end 


function CommonAlert:showItem(item,msg)
	-- local rtf = Util.Find(item,"Text","Lui.LRichText")
	-- rtf.defaultLabColor = msg.color
	-- rtf:parseRichDefaultString(msg.content,nil)	
	-- rtf.transform.localPosition = Vector2(0 - rtf.realLineWidth / 2,11)

	if not item or GameObject.__eq(item,nil) then return end
	
	local rtf = Util.Find(item,"content","Text")
	rtf.text = tostring(msg.content)

	LeanTween.delayedCall(rtf.gameObject,0.05,System.Action(function()
		if rtf.preferredWidth > 300 then
			local offsetWidth = rtf.preferredWidth - 300
			Util.Find(item,"bg","RectTransform").sizeDelta = Vector2(408 + offsetWidth,46)
		else
			Util.Find(item,"bg","RectTransform").sizeDelta = Vector2(408,46)
		end
	end))

	local commItem = Util.Find(item,"bg/CommItem")
	if msg.baseId then
		commItem:SetActive(true)
	else
		commItem:SetActive(false)
	end
end

--多条时，创建prefab
function CommonAlert:getCacheItem()
	local item = nil
	for _,struct in ipairs(self.cache_list) do
		if not struct.isUse then
			item = struct.obj
			item:SetActive(true)
			struct.isUse = true
			break
		end
	end
	if item == nil then
		item = GameObject.Instantiate(self.inje_AlertItem)
		table.insert(self.cache_list,{obj=item,isUse=true})
	end
	return item
end

function CommonAlert:RecycleItem(item)
	for _,struct in ipairs(self.cache_list) do
		if struct.obj == item then
			struct.obj:SetActive(false)
			struct.isUse = false
			break
		end
	end
end

-- function CommonAlert:OnDestroy()
-- 	Util.CleanRef(self.pop_msgs)
-- 	Util.CleanRef(self.item_list)
-- 	Util.CleanRef(self.cache_list)
-- end
