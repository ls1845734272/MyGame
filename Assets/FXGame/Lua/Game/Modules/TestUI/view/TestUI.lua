
TestUI = class(UIBase,"TestUI")



TestUI.inje_Image = 1 
TestUI.inje_Button = 1 
TestUI.inje_CloseBtn = 1


function TestUI:Awake()

end 

function TestUI:OpenAnim()
	self:PlayAnimOpen()
end


function TestUI:Open()
  print("打开了测试界面Open1111111111111111111111")



  self.inje_CloseBtn.onClick:AddListener(function() 
  	self:PlayAnimClose(function() 
        print("关闭了界面")
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

function TestUI:setTestConfig( tab )
  -- body
end

function TestUI:SetLeanTween(obj)
  local pos1 = Vector3(0.7,0.7,1)
  --延迟0.6秒执行
  LeanTween.delayedCall(0.6,System.Action(function()   end))

  --下一帧执行
  local function nextFrameProcess()
   
  end
  CallDelay:nextCall(nextFrameProcess)

  -- StartCoroutine(function()
  --     WaitForEndOfFrame()
  --     Yield(0)
  --     print("携程")
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