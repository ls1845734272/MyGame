
local M = class("UILayer")

_G.UILayer = M;


function M:ctor(data)
	self.childs = {};
	self.isHide = false;
	self.startOrder = 0 --开始层级

	if data then
		self.index = data.index
		self.name = data.name
	end
end

function M:init()

end

function M:childBottom(display)
	if display.gameObject then
		-- display.gameObject.transform:SetSiblingIndex(0)
		display.gameObject.transform:SetAsLastSibling()
	end
end

function M:removeChild( display,isDontDestroy )
	assert(display,"display is nil!")
	if not isDontDestroy and display.gameObject then
		GameObject.Destroy(display.gameObject)
	else
		display:SetActive(false)
	end
	if table.include(self.childs,display) then
		local i = table.indexOf(self.childs,display)
		table.remove(self.childs,i)
	end
end


function M:addChild( display )
	assert(display,"display is nil!")
	if table.include(self.childs,display) then
		return		
	end

	display:SetActive(true)
    display.transform:SetAsLastSibling()

	if self.gameObject and display.gameObject then
		display.transform:SetParent(self.gameObject.transform,false)
		table.insert(self.childs,display)
	end

	if self.name ~= "MainUILayer" then 
        self:layerOrder(display)
	end 
end

function M:layerOrder(display)
	local order = display:GetComponent(typeof(SortingOrder))
	if not order then
		order = display:AddComponent(typeof(SortingOrder))
		order.isUI = true	
	end

	--思路
	--[[
		当前预制体界面 层级
		先根据索引加一个order 
		当前界面下的sortOrder 再以此添加索引
	]]
	local index = table.indexOf(self.childs,display)
	if not index then
		index = display.transform:GetSiblingIndex() --当前索引
	end
	local uiCount = self.gameObject.transform.childCount -- 当前层子数量

	order.sortingOrder = (index) * 50
	self:updateOrder(display)

end 

--更新order
function M:updateOrder(display)
	--printStack()
	assert(display,"display is nil!")	
	if display then
		local orderRenderers = display:GetComponentsInChildren(typeof(SortingOrder),true);
		-- print("当前面板SortingOrder数量",orderRenderers.Length)
		if orderRenderers.Length > 0 then
			local render = nil
			for i=0,orderRenderers.Length-1 do
				render = orderRenderers[i]
				if render and render.gameObject then
					render:UpdateSortingOrder()
				end
			end
		end
	end
end


function M:updateAllSortingOrder()

	if self.name~='MainUILayer' then
		for i,v in ipairs(self.childs) do
			if v and not v:Equals(nil) then
				local order = v:GetComponent(typeof(SortingOrder))
				if not order then
					order = v:AddComponent(typeof(SortingOrder))
					order.isUI = true	
				end
				order.sortingOrder = (i) * 50

				local luaBehaviour = v:GetComponent("LuaBehaviour")
				if luaBehaviour then
					local luaTable = luaBehaviour:GetInstance()
					if luaTable._maskLayer then
						local maskSo = luaTable._maskLayer:GetComponent(typeof(SortingOrder))
						maskSo.sortingOrder = order.sortingOrder -1
					end
				end
				self:updateOrder(v)
			end
		end
	end
end


return M;