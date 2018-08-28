UIUtils = UIUtils or {}

function UIUtils:LoadUI(path,func)
	local obj = CLuaUtil.LoadInstantiate(path)
	if obj then
		func(obj)
	end
end


function UIUtils:LoadBaseUI(path,func)
	CLuaUtil.LoadUIAsync(path,func)	
end

function UIUtils:strIsEmptyOrNull(str)
    return str == nil or str ==''
end 



------------------------------------str-----------------------------------
--解析字符串 分割   string.split();一样的
function UIUtils:strSplit(szFullString, szSeparator)
	local nFindStartIndex = 1
	local nSplitIndex = 1
	local nSplitArray = {}
	while true do
	   local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)
	   if not nFindLastIndex then
	    nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))
	    break
	   end
	   nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1)
	   nFindStartIndex = nFindLastIndex + string.len(szSeparator)
	   nSplitIndex = nSplitIndex + 1
	end
	return nSplitArray
end

---------------------------------------------数字转化-------------------

--数字转中文0-999
function UIUtils:numToCN(num)
    if num <= 10 then 
        return self:singleNumToCN(num)
    end 

    if num > 10 and num <100 then 
    	local f = math.floor(num/10)
    	local b = num%10
        return self:singleNumToCN(f).."十"..self:singleNumToCN(b)
    end
    
    if num >=100 then 
    	local t = math.floor(num/100)
    	local f = math.floor(num/10)
    	local b = num%10
    	return self:singleNumToCN(t).."百"..self:singleNumToCN(f).."十"..self:singleNumToCN(b)
    end 
    log("<color=red>ERROR 数字大于999 不支持</color>")
end

--数字转中文数字 1->一 2->二
function UIUtils:singleNumToCN(num) 
	local cnNum = ""	
	if num == 1 then 
		cnNum = '一'
	elseif num ==2 then 
	    cnNum = '二'
	elseif num ==3 then 
	    cnNum = '三'
	elseif num ==4 then 
	    cnNum = '四'
	elseif num ==5 then 
	    cnNum = '五'
	elseif num ==6 then 
	    cnNum = '六'
	elseif num ==7 then 
	    cnNum = '七'
	elseif num ==8 then 
	    cnNum = '八'
	elseif num ==9 then 
	    cnNum = '九'
	elseif num ==0 then 
	    cnNum = '零'
	elseif num == 10 then 
		cnNum = "十"
    else
    	logError("0-10没有这个数字")
	end
	return cnNum
end



-----------------------------------------------------------------------------------------
--[[
	** 设置界面是否可见
	go gameobject
	visible 是否可见
]]
function UIUtils:setVisible(go,visible,backPos,isNotReplacePos)
	if not go then return end
	if go:Equals(nil) then return end
	-- if not go.gameObject then return end
	visible = visible==nil and true or visible
	local cg = go.gameObject:GetComponent(typeof(UnityEngine.CanvasGroup))
	if not cg then
		cg = go.gameObject:AddComponent(typeof(UnityEngine.CanvasGroup))
	end
	if visible then
		-- go.layer = LayerMask.NameToLayer("UI")
		if isNotReplacePos == nil then
			go.transform.localPosition = backPos or Vector3.zero
		end
		cg.alpha = 1
	else
		-- go.layer = LayerMask.NameToLayer("UIHide")
		if isNotReplacePos == nil then
			go.transform.localPosition = Vector3(0,5000,0)
		end
		cg.alpha = 0
	end
end

return UIUtils






