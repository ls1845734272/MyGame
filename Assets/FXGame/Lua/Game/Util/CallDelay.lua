

_G.CallDelay = CallDelay or {}

local UpdateBeat = UpdateBeat


--[[

	延迟调用

--]]
function CallDelay:init()
	CallDelay._callDic = {};
	CallDelay._nextCall = {};
	CallDelay._nextCallQueue = {};
	CallDelay.isStart = false;
end

--[[
	@callObj 
	@fun    	方法
	@delay  	延迟毫秒
	@loop 		是否循环
    @data		数据
	return :返回一个id,可用来清除绑定
--]]
function CallDelay:delayCall(callObj,fun,delay,loop,data)
	if(not fun)then return end;
	local intervalId = GlobalUtil:getUID();
	CallDelay._callDic[intervalId] = {callObj,fun,(Time.time*1000 + delay),loop,delay,data};
	self:check();

	return intervalId;

end

--改变定时器时间
function CallDelay.setTimer(id,delay)
	local tt = CallDelay._callDic[id];
	if(tt)then
		tt[5] = delay;
	end
end

--改变定时器循环
function CallDelay.setloop(id,loop)
	local tt = CallDelay._callDic[id];
	if(tt)then
		tt[6] = loop;
	end
end

-- 在此调用未完成前重置它
function CallDelay.resertCall(id)
	local tt = CallDelay._callDic[id];
	if(tt)then
		tt[3] = Time.time*1000 + tt[5];
	end
end

--删除绑定
function CallDelay:deleted(id)
	if(not id)then return end;

	CallDelay._callDic[id] = nil;

	self:check();
end
	
--删除绑定	
function CallDelay:cleanCall(callObj, fun)
	if(not fun)then return end;
	for key, v in pairs(CallDelay._callDic) do  
		if (v[1] == callObj and v[2] == fun)then
			CallDelay._callDic[key] = nil;
			break;
		end
	end

	self:check();
end

function CallDelay:check()
	local bol = next(CallDelay._callDic);

	if(bol ~= nil)then
		if(CallDelay.isStart == false)then
			CallDelay.isStart = true;

			UpdateBeat:Add(self.render,self)
			-- RenderManager:addCall("callDay_t_render",CallDelay.render)	
		end
	else
		-- print("calldelay stop");
		-- RenderManager:removeCall("callDay_t_render",CallDelay.render);
		UpdateBeat:Remove(self.render, self)
		CallDelay.isStart = false;
	end
end

function CallDelay:render()
	local num = 0;
	for key, v in pairs(CallDelay._callDic) do 
		num = num + 1;
		if(v and v[3] < Time.time*1000)then
			if(type(v[2]) == "function")then
				local function erFun(msg)
					print(v.intervalId.."[Error]:\n\t\t"..msg)
					print(debug.traceback())
				end
				xpcall(function()
					if(v[6])then
						v[2](v[1],v[6])
					else
						v[2](v[1])
					end
				end,erFun);

			end

			if(v[4] == true)then
				v[3] = Time.time*1000 + v[5];
			else
			   --移除绑定
			   -- print("移除定时器：" .. key)
			   CallDelay._callDic[key] = nil;
			end
		end
	end

	-- print("拥有定时器：" .. num)

	local bol = next(CallDelay._callDic);
	if(bol == nil)then
		UpdateBeat:Remove(self.render, self)
		-- RenderManager:removeCall("callDay_t_render",CallDelay.render);
		CallDelay.isStart = false;
		-- print("calldelay stop");
	end
end

--下一帧执行
function CallDelay:nextCall(func,callObj,data)
	-- self.nextT = Time.time
	-- table.insert(self._nextCall,{func=func,callObj=callObj,data=data})
	-- if not self.timer then
 --        self.timer = Timer.New()
 --    end
    local timer = Timer.New(function()
        func(callObj,data)
    end,0,1)
    timer:Start()
end

-- function CallDelay:doNextCall()
-- 	local callInfo = nil
-- 	for k,v in pairs(self._nextCall) do
-- 		callInfo = v 
-- 		if callInfo and callInfo.func then
-- 			callInfo.func(callInfo.callObj,callInfo.data)
-- 		end
-- 	end
-- 	TableUtil.removeAll(self._nextCall)
-- end

function CallDelay:nextCallQueue(func,data)
	table.insert(self._nextCallQueue,{func=func,data=data})
end

function CallDelay:startNextCallQueue()
	UpdateBeat:Remove(self._doNextCallQueue,self)
	UpdateBeat:Add(self._doNextCallQueue,self)
end

function CallDelay:_doNextCallQueue()
	if #self._nextCallQueue > 0 then
		local v = self._nextCallQueue[1]
		v.func(v.data)
		table.remove(self._nextCallQueue,1)
	else
		UpdateBeat:Remove(self._doNextCallQueue,self)
	end
end

CallDelay:init();	

return M;