ViewLayerDefault = class(UILayer,"ViewLayerDefault")

local M = ViewLayerDefault

function M:addChild( display,viewId )
	self:super("UILayer","addChild",display)
 
    --dump("viewlayerDefault de AddClide")
	if viewId then
		if UIViewID[viewId].fullScreen == 2 then
			Util.adpPanelUI(display,true)
		end
	end
end

function M:removeChild( display,isDontDestroy,viewId,layerId)
	self:super("UILayer","removeChild",display,isDontDestroy)
end