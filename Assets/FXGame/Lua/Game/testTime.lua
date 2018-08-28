
-- local getTimeZoneDiff = function()
-- 	local now = os.time()
-- 	return 28800 - os.difftime(now, os.time(os.date("!*t", now)))
-- end



-- local isdst = os.date("*t",os.time()).isdst
-- print(isdst,"夏令")
-- print(getTimeZoneDiff(),"差值")
-- local now = os.time() -- 标准时间时间戳格式
-- print(now,"标准戳")
--print(os.time(os.date("!*t", now)),"当前戳")

-- local curzonedate =  os.date("!*t", os.time())
-- print("0时data")
-- for i,v in pairs(curzonedate) do 
--    print(i)
--    print(v)
-- end 

-- local curzonedate2 =   os.date("*t", os.time())
-- print("当前data")
-- for i,v in pairs(curzonedate2) do 
--    print(i)
--    print(v)
-- end 


-- print(math.floor(51/50))

-- local kk ="2018-7-27 23:59:59"
-- --local endTime = Util.strToDate(kk)
-- local curIsdst = false--os.date("*t",os.time()).isdst
-- --local curIsdst = false
-- print(curIsdst,"00000000000000000000000000000000000")
-- local endTimestr = os.time({year = 2018,month = 7,day = 27,hour=23,min=59,sec=59,isdst = curIsdst})
-- print(endTimestr,"111111111111111111111111111111111111111111111")
