
TestUI = class(UIBase,"TestUI")


TestUI.inje_Image = 1 
TestUI.inje_Button = 1 
TestUI.inje_CloseBtn = 1
TestUI.inje_Text = 1
TestUI.inje_fill = 1
TestUI.inje_sliderValue = 1

function TestUI:Awake()

end 

function TestUI:OpenAnim()
	self:PlayAnimOpen()
end

function TestUI:Open()
    print("打开了测试界面Open1111111111111111111111")
    self:AddEvents()
    
    print("测试新分支提交")
    -- self:SetShowText()
    -- self:nextFrame()
end 

function TestUI:SetShowText()
    -- self.inje_Text.text = string.format( "<color=#7f7868>%s</color>","0000")--"123456789"
    -- self.inje_Text.color = toColor("FFFFFF")
    -- local str = HtmlTextUtil.addColor("helloworld",toColor("FFFFFF").hex)
    -- self.inje_Text.text = str
    self:playBreathAni(self.inje_Text.gameObject)
    self:SetSlider()
end 

--添加事件 Button
function TestUI:AddEvents()
    -- body
    self.inje_CloseBtn.onClick:AddListener(function() 
        self:PlayAnimClose(function() 
            --ModuleManager:sendModule(ModuleType.TestUI)
            UIManager.GetInstance():HideView("TestUI")
        end)
    end)
    local btn = self.inje_Button.gameObject:GetComponent("Button")
    btn.onClick:AddListener(function() 
        Dispatcher.dispatchEvent(EventType.FIEST_EVENT)
    end)
    -- local instance = self.gameObject:GetComponent('LuaBehaviour')
    -- instance:AddClick(self.inje_Button.gameObject,function()    
    --    print("iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii")
    -- 	end)
end

--设置slider
function TestUI:SetSlider()
    local curAmount = 10
    local maxAmount = 15
    if self.timer2 then self.timer2:Stop() self.timer2 = nil end 
    self.timer2 = Timer.New(function() 
        self.inje_sliderValue.text = string.format( "%s/%s",curAmount,maxAmount)
        local per = curAmount/maxAmount

        self:ImageDoFill(self.inje_fill,per)
        curAmount = curAmount + 1
    end,3,-1):Start()
end 

--处理延迟的方法
function TestUI:nextFrame()
    if self.timer then self.timer:Stop() self.timer = nil end 
    self.timer = Timer.New(function() 
        local str = "好孩子"
        local itemColor = ColorType.getItemColor(4).hex  
        UIManager.ShowAlert(string.format("我们都是%s",HtmlTextUtil.addColor(str,itemColor)),nil,nil,false)
    end,3,-1):Start()

    -- --延迟0.6秒执行
    -- LeanTween.delayedCall(0.6,System.Action(function()   end))

    --下一帧执行
    -- local function nextFrameProcess()
    --     print("00000000000000000")
    -- end
    -- CallDelay:nextCall(nextFrameProcess)

    --协程
    -- StartCoroutine(function()
    --     -- WaitForEndOfFrame()
    --     -- Yield(0)
    --     print("000000000")
    --     WaitForSeconds(1)
    --     print("11111111")
    --     WaitForSeconds(1)
    --     print("22222222222")
    --     -- print(i,"携程i")
    -- end)
end 


function TestUI:Close()

end 



















-- -- AdvanceMount脚本
-- TestUI.queueList = {}
-- TestUI.isPlayQueue = false
-- function TestUI:DoFill(img1,endValue1)
--   table.insert(self.queueList,{img = img1,endValue = endValue1})
--   self:PlayFill(img1,endValue1)
-- end

-- function TestUI:PlayFill(img,endValue)
--   -- body
--   self.isPlayQueue = true
--   --self.inje_guang.gameObject:SetActive(false)
--   if endValue>img.fillAmount then
--     img:DOFillAmount(endValue,0.1):OnComplete(function()
--       -- body
--       table.remove(self.queueList,1)
--       if(#self.queueList>0) then
--         self:PlayFill(self.queueList[1].img,self.queueList[1].endValue)
--       else
--         self.isPlayQueue = false
--       end
--     end)
--   else
--     img:DOFillAmount(1,0.05):OnComplete (function()
--       img.fillAmount = 0
--       img:DOFillAmount(endValue,0.05):OnComplete(function()
--         table.remove(self.queueList,1)
--         if(#self.queueList>0) then
--           self:PlayFill(self.queueList[1].img,self.queueList[1].endValue)
--         else
--           self.isPlayQueue = false
--         end
--       end)
--     end)
--   end
-- end