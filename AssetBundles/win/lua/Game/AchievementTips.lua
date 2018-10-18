AchievementTips = SimpleClass(UIDisplay)

function AchievementTips:initLayout()
	self.timer = nil 
    self.tempItem = self.transform:Find("tempItem")
	self.achText =  self.transform:Find("tempItem/ui_dacheng/achText"):GetComponent("Text")
	self.achText2 =  self.transform:Find("tempItem/ui_dacheng/achText/achText (1)"):GetComponent("Text")
    self.cilckBtn = self.transform:Find("click/cilckBtn"):GetComponent("Button")
    self.parSys =  self.transform:Find("tempItem/ui_dacheng/kuangjia"):GetComponent("ParticleSystem")
    self.anim =  self.transform:Find("tempItem/ui_dacheng/kuangjia/ui_saoguang_1"):GetComponent("Animation")
end 

function AchievementTips:_playEffect()
	if self.parSys then 
		    xpcall( function( )
				self.parSys:Play()
				if self.anim then 
					CMusicManager:PlayUIMusic("ui_open102.ogg")	
					self.anim:Play()
				end 
				end,function()
				if  UnityEngine.Application.isEditor then
				log("error:"..debug.traceback())
				end
			end)
		end 
end

function AchievementTips:onRefresh()
   local m = ModuleManager:getModule(ModuleType.Achievement)
   if m == nil then return end 

	dump(m.queue:getLst(),"m.queue:getLs")
	
	if m.queue:isEmpty() then 
		print("成就为空，清空预制")
   	  UIUtils:hideTips(UIUtilsTips.AchievementTips,true)
   	  return 
	end 
	self.cilckBtn.onClick:AddListener(function()
        Dispatcher.dispatchEvent(EventType.Open_Achievement_UI,nil,self.data)
	end)
	if not self.parSys then
		print("iiiiiiiiiiiiiiiiiiii")
		--  UIUtils:LoadBaseUI("Prefabs/UI/Common/ui_eff_boom.prefab",function(go)
		--  	if go then
		-- 	 	go.transform:SetParent(self.tempItem)
		-- 	 	go.transform.localPosition = Vector3.zero 
		-- 	 	go.transform.localScale = Vector3.one
		-- 	 	self.parSys = go.transform:Find("fangkuang"):GetComponent('ParticleSystem')
		-- 	 	self:_playEffect()
		-- 	 end
		--  end)
	else
		self:_playEffect()
	end 
	
	LeanTween.cancel(self.achText.gameObject)
   	local canvas = self.achText.gameObject:GetComponent("CanvasGroup")
	canvas.alpha = 1
	local code = m.queue:pop()
	dump(code,"当前成就的code")
	self.data = m:getSingleData(code)
	self.achText2.text = "成就达成"
	self.achText.text = self.data:getAchName()
	if self.timer then self.timer:Stop() self.timer = nil end 
    self.timer = Timer.New(function() 
		  --LeanTween.alphaCanvas(canvas,0,0.2):setOnComplete(System.Action(function()
			 print("执行了几次")
	      	 self:onRefresh()
	      --end))
    end,4.6):Start()
    if self.timer2 then self.timer2:Stop() self.timer2 = nil end 
	self.timer2 = Timer.New(function() 
			  if  canvas:Equals(nil) then
				return
			  end 
			  LeanTween.alphaCanvas(canvas,0,0.28):setOnComplete(System.Action(function()
				print("一个成就的结束")
				-- if self.transform then 
				--     self.transform.localPosition = Vector3(0,5000,5000)
				-- end 
				self.achText2.text = ""
				self.achText.text =  ""
				--self.tempItem.gameObject:SetActive(false)
				self.cilckBtn.onClick:RemoveAllListeners()
		   end))
    end,1.4):Start()

    if not self._hasScale then
		self._hasScale = true
		self.transform.localScale = Vector3.one * Util.getAdpTargetScale() 
	    -- self.transform:Find("tempItem/ui_dacheng/kuangjia").transform.localScale =self.transform:Find("tempItem/ui_dacheng/kuangjia").transform.localScale * GlobalUtil.getResolution()
	    -- self.transform:Find("tempItem/ui_dacheng/kuangjia/dian").transform.localScale =self.transform:Find("tempItem/ui_dacheng/kuangjia/dian").transform.localScale * GlobalUtil.getResolution()
	    -- self.transform:Find("tempItem/ui_dacheng/kuangjia/guangxian").transform.localScale =self.transform:Find("tempItem/ui_dacheng/kuangjia/guangxian").transform.localScale * GlobalUtil.getResolution()
	    -- self.transform:Find("tempItem/ui_dacheng/kuangjia/g1").transform.localScale =self.transform:Find("tempItem/ui_dacheng/kuangjia/g1").transform.localScale * GlobalUtil.getResolution()
	    -- self.transform:Find("tempItem/ui_dacheng/kuangjia/g2").transform.localScale =self.transform:Find("tempItem/ui_dacheng/kuangjia/g2").transform.localScale * GlobalUtil.getResolution()
	    -- self.transform:Find("tempItem/ui_dacheng/kuangjia/kuangtiao").transform.localScale =self.transform:Find("tempItem/ui_dacheng/kuangjia/kuangtiao").transform.localScale * GlobalUtil.getResolution()
	end
end

function AchievementTips:onOpen()
	-- self.cilckBtn.onClick:AddListener(function()
    --     Dispatcher.dispatchEvent(EventType.Open_Achievement_UI,nil,self.data)
	-- end)
end

function AchievementTips:onClose()
	--if self.timer then self.timer:Stop() self.timer = nil end 
	--print("iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii")
    --self.cilckBtn.onClick:RemoveAllListeners()
end

function AchievementTips:onDispose(args)
	if self.timer then self.timer:Stop() self.timer = nil end 
	if self.timer2 then self.timer2:Stop() self.timer2 = nil end 
	self.isInit = nil 
	self.data = nil 
	self.transform = nil 
	self.parentUI = nil
	self.gameObject =nil 
end

function AchievementTips:OnDestroy()
	--Util.CleanRef(self)
end
