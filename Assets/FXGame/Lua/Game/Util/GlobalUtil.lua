GlobalUtil = GlobalUtil or {}
GlobalUtil._timerDiff = 0

local __id__ = 100000000

--获取全局唯一的id (Unique ID)
function GlobalUtil:getUID()
	__id__ = __id__ + 1
	return __id__
end