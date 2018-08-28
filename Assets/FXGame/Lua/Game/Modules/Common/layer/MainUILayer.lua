MainUILayer = class(UILayer,"MainUILayer")

local M = MainUILayer

function M:addChild( display,viewId )
	-- body
	self:super("UILayer","addChild",display)
		if #self.childs == 4 then
		local nameList = {"MainTopRight","MainTopLeft","MainBottomRight","MainBottomLeft"}
		for _,name in ipairs(nameList) do
			for _,child in ipairs(self.childs) do
				if child.name == name then
					self:childBottom(child)
				end
			end
		end
	end
end