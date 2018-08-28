ViewLayer = class(UILayer,"ViewLayer")

local M = ViewLayer

function M:ctor(data)

	self.fullScreenViewIds = {}
end


function M:addChild( display,viewId )
	self:super("UILayer","addChild",display)

    if viewId then
        local viewConfig = UIViewID[viewId]
		local sca = Screen.width/Screen.height
		--if viewConfig.isEAdapt and sca >= 1.9 and sca<=2.2 then
			display.transform.localScale = Vector3(1,1,1)
			local rtran = display:GetComponent("RectTransform")
			rtran.pivot = Vector2(0.5, 0.5)
			rtran.anchorMax = Vector2(0.5, 0.5)
			rtran.anchorMin = Vector2(0.5, 0.5)
			rtran.sizeDelta = Vector2(1280,720)
		--else
			--处理适配

			-- Util.adpPanelUI(display,true,UIViewID[viewId].fullScreen == 1 and  
			-- 	(viewId ~= "EffectiveWeaponView" and 
			-- 	 viewId ~= "EWeaponItemPage" and 
			-- 	 viewId ~= "UnionDominate" and
			-- 	 viewId ~= "PracticePageGodBoard"))
		--end
    end 
end 


function M:removeChild( display,isDontDestroy,viewId,layerId,isDelet)
	self:super("UILayer","removeChild",display,isDontDestroy)

	if viewId and layerId then
		if layerId == "ViewLayer" then

			-- if not isDelet then
			-- 	self:checkSceneMask()
			-- end
		end
	end
end