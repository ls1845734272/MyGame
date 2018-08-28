Util = Util or {};


--处理分辨率变化，
function Util.getAdpTargetScale()
    local ap = Screen.width / Screen.height
    if ap < 1.7 then --ipad
        return ap / (1280 / 720)
    else
        return 1
    end
end

function dump(value, desciption, nesting)
    if type(nesting) ~= "number" then nesting = 3 end
    -- printStack()
    local lookupTable = {}
    local result = {}

    local function _v(v)
        if type(v) == "string" then
            v = "\"" .. v .. "\""
        end
        return tostring(v)
    end

    -- local traceback = string.split(debug.traceback("", 2), "\n")
    -- print("dump from: " .. string.trim(traceback[3]))

    local function _dump(value, desciption, indent, nest, keylen)
        desciption = desciption or "<var>"
        local spc = ""
        if type(keylen) == "number" then
            spc = string.rep(" ", keylen - string.len(_v(desciption)))
        end
        if type(value) ~= "table" then
            result[#result +1 ] = string.format("%s%s%s = %s", indent, _v(desciption), spc, _v(value))
        elseif lookupTable[value] then
            result[#result +1 ] = string.format("%s%s%s = *REF*", indent, desciption, spc)
        else
            lookupTable[value] = true
            if nest > nesting then
                result[#result +1 ] = string.format("%s%s = *MAX NESTING*", indent, desciption)
            else
                result[#result +1 ] = string.format("%s%s = {", indent, _v(desciption))
                local indent2 = indent.."    "
                local keys = {}
                local keylen = 0
                local values = {}
                for k, v in pairs(value) do
                    keys[#keys + 1] = k
                    local vk = _v(k)
                    local vkl = string.len(vk)
                    if vkl > keylen then keylen = vkl end
                    values[k] = v
                end
                table.sort(keys, function(a, b)
                    if type(a) == "number" and type(b) == "number" then
                        return a < b
                    else
                        return tostring(a) < tostring(b)
                    end
                end)
                for i, k in ipairs(keys) do
                    _dump(values[k], k, indent2, nest + 1, keylen)
                end
                result[#result +1] = string.format("%s}", indent)
            end
        end
    end
    _dump(value, desciption, "- ", 1)

    for i, line in ipairs(result) do
        print(line)
    end
end
    
--获得函数的执行时长 单位毫秒ms
function getCostTime(func,...)
    if not func then return end
    
    if type(func)=="function" then
        local cost = _tick()
        func(...)
        return _tick() - cost
    end
    return 0
end

Util.Find = function(gameObject,path,type)
    if(not gameObject)then return nil end;
    local tran = gameObject.transform:Find(path)
    if tran == nil then
        -- log("path:"..path.." 为null")
        return
    end
    local obj = tran.gameObject
    if type == nil then
        return obj
    else
        return obj:GetComponent(type)
    end
end

-- 清空子节点
Util.removeAllChild = function(gameObject)
    
    local len = gameObject.transform.childCount
    local obj = nil
    for i=0,len-1 do
        obj = gameObject.transform:GetChild(i).gameObject
        Object.Destroy(obj)
    end

end

-- 清空子节点
Util.removeAllChildByName = function(gameObject,name)
    
    local len = gameObject.transform.childCount
    local obj = nil
    for i=0,len-1 do
        obj = gameObject.transform:GetChild(i).gameObject
        if obj.name == name then
            Object.Destroy(obj)
        end
    end
end

------------------------------------处理时间的方法--------------------
-------------------------------------处理时间要考虑夏令时与非夏令时----
--将"2017-01-01 21:00:00.000"转换为date格式
Util.strToDate = function(str,param)

    if not str then
        print("时间为空")
        return
    end

    if type(str) ~= "string" then
        local curTime = TimeManager.cur_time
        local now = os.date("*t",curTime)
        return now
    end

    local date = {}
    local arr = string.split(str," ")
    local arr1 = string.split(arr[1],"-")
    date.year = tonumber(arr1[1])
    date.month = tonumber(arr1[2])
    date.day = tonumber(arr1[3])
    local arr2 = string.split(arr[2],":")
    date.hour = tonumber(arr2[1])
    date.min = tonumber(arr2[2])
    date.sec = tonumber(arr2[3])

    local curTime = TimeManager.cur_time
    local now = os.date("*t",curTime)
    
    if param == true then --今天
        date.year = now.year
        date.month = now.month
        date.day = now.day
    elseif type(param) == "number" then --周几
        date.year = now.year
        date.month = now.month
        param = param + 1
        if param == 8 then
            param = 1
        end

        if now.wday >= param then
            date.day = now.day - (now.wday - param)
        else
            date.day = now.day + (param - now.wday)
        end
    end
    return date
end

--将date转换成时间戳
Util.DataToSeconds = function(date)
    -- body
    local time = os.time({day = date.day,
        month = date.month,
        year = date.year,
        hour = date.hour,
        min = date.min,
        sec = date.sec,
        isdst = date.isdst})
    return time
end

--时间的显示
Util.formatLeftTime = function(seconds)    
    if seconds>=3600 then 
        return Util.formatLeftTime1(seconds)
    else
        return Util.formatLeftTime2(seconds)
    end
end

Util.formatLeftTime1 = function(seconds)

    local hour = math.floor(seconds / 60 / 60)
    local min = math.floor(seconds / 60) % 60
    local second = seconds % 60
    return string.format("%02d:%02d:%02d",hour,min,second)
end

Util.formatLeftTime2 = function(seconds)
    local min = math.floor(seconds / 60)
    local second = seconds % 60
    return string.format("%02d:%02d",min,second)
end



Util.formatLeftTime3 = function(seconds)    
    --86400
    local rtn = ''
    if seconds > 86400 then --如果大于天 返回xx天xx小时xx分钟xx秒
       local day = math.floor(seconds/86400) 
       local hour = math.floor((seconds%86400)/3600) 
       local min = math.floor(((seconds%86400)%3600)/60)
       local sec = math.floor(((seconds%86400)%3600)%60)  
       rtn = string.format("%d天%02d时%02d分%02d秒",day,hour,min,sec)       
    elseif seconds>3600 then --如果大于1小时 返回xx小时xx分钟xx秒
       local hour = math.floor(seconds/3600) 
       local min =  math.floor((seconds%3600)/60)
       local sec =  math.floor((seconds%3600)%60)
       rtn = string.format("%d小时%02d分%02d秒",hour,min,sec)
    elseif seconds>60 then --如果大于1分支 返回xx分钟xx秒
        local min = math.floor(seconds/60) 
        local sec = math.floor(seconds%60)
        rtn = string.format("%02d分%02d秒",min,sec)
    else
        local sec = seconds
        rtn = string.format("00分%02d秒",sec)
    end    
    return rtn
end

--获得获取从1970-1-1 0:0:0起总天数（os.time的起始时间是1970-1-1 8点）
Util.GetTotalDay = function(seconds)
    local kk = (seconds +(8*60*60)) /86400
    local nowDay = math.ceil(kk)
end 

-------------------------------------------
Util.strNumToCh = function(num)
    -- body
    if num == 1 then
        return "一"
    elseif num == 2 then
        return "二"
    elseif num == 3 then
        return "三"
    elseif num == 4 then
        return "四"
    elseif num == 5 then
        return "五"
    elseif num == 6 then
        return "六"
    elseif num == 7 then
        return "七"
    elseif num == 8 then
        return "八"
    elseif num == 9 then
        return "九"
    else
        return ""
    end
end

-- 格式化消息数字的显示
function Util.getTipsNumStr(num)
    if num > 99 then
        return "99+"
    else
        return tostring(num)
    end
end

-------------------------------------------------------------

function Util.FileDelete(path)
    if System.IO.File.Exists(path) then
        System.IO.File.Delete(path)
    end
end

--字符串长度是否满足
function Util.StringLengthMeet(str,length)
    local count = 0  
    for uchar in string.gfind(str, "([%z\1-\127\194-\244][\128-\191]*)") do   
        if #uchar ~= 1 then  
            count = count +2  
        else  
            count = count +1  
        end  
    end
    return count <= length    
end

--判断是否全部为中文
function Util.checkIsAllCh(str)
    if #str > 0 and #str/3 % 1 == 0 then
        --判断是否全部为中文
        for i=1,#str,3 do
            local tmp = string.byte(str, i)
            --print(" ----   ----",tmp)--230
            if tmp >= 240 or tmp < 224 then
                return false
            end
        end
        --判断中文中是否有中文标点符号
        for i=1,#str,3 do
            local tmp1 = string.byte(str, i)
            local tmp2 = string.byte(str, i+1)
            local tmp3 = string.byte(str, i+2)
            --print("mp1,mp2,mp3",tmp1,tmp2,tmp3)
            --228 184 128 -- 233 191 191
            if tmp1 < 228 or tmp1 > 233 then
                return false
            elseif tmp1 == 228 then
                if tmp2 < 184 then
                    return false
                elseif tmp2 == 184 then
                    if tmp3 < 128 then
                        return false
                    end
                end
            elseif tmp1 == 233 then
                if tmp2 > 191 then
                    return false
                elseif tmp2 == 191 then
                    if tmp3 >191 then
                        return false
                    end
                end            
            end
        end
        return true
    end
    return false
end

----------------------------------加载图片-------置灰的处理---------------------
--异步加载中图片
function Util.IsImage(image)
    if image and type(image) == 'userdata' and not image:Equals(nil) 
        and (image:GetType():ToString() == 'UnityEngine.UI.Image' or
            image:GetType():ToString() == 'UnityEngine.UI.RawImage') then
        return true
    end
    return false
end

function Util.EnableGrayImage(image)
   if not Util.IsImage(image) then return end 


end 

---------------------------------------------------------------------------------------
-- -- 自适配ui ipad / 手机
function Util.adpPanelUI(gameObject,isAdpIphoneX,isBaseBg)

--     local isIpad = false
--     local ap = Screen.width / Screen.height
--     if ap < 1.7 then -- 方形屏幕

--         isIpad = true

--         local rtran = gameObject:GetComponent("RectTransform")
--         rtran.pivot = Vector2(0.5, 0.5)
--         rtran.anchorMax = Vector2(0.5, 0.5)
--         rtran.anchorMin = Vector2(0.5, 0.5)
--         rtran.sizeDelta = Vector2(1280,720)

--         local s = ap / (1280 / 720)

--         gameObject.transform.localScale = Vector3(s,s,s)
--     else -- 长形屏幕
--         local rtran = gameObject.transform
--         rtran.anchorMin = Vector2.zero
--         rtran.anchorMax = Vector2.one 
--         rtran.offsetMin = Vector2.zero
--         rtran.offsetMax = Vector2.zero
--         gameObject.transform.localScale = Vector3(1,1,1)

--         -- iphoneX 适配
--         if UIUtils.isIphoneX() and isAdpIphoneX  then

--             local rtran = gameObject.transform
--             if isBaseBg then
--                 local baseBg = gameObject.transform:Find("__baseBg")
--                 if not baseBg then
--                     UIUtils:LoadBaseUI("Prefabs/UI/Common/CommBgX.prefab",function(baseBg)
--                         if not rtran:Equals(nil) then
--                             baseBg.transform:SetParent(rtran.transform)
--                             baseBg.transform:SetAsLastSibling()
--                             local rt = baseBg:GetComponent("RectTransform")
--                             rt.anchorMin = Vector2.zero
--                             rt.anchorMax = Vector2.one 
--                             rt.offsetMin = Vector2(-Util.getLiuHaiValue(),0)
--                             rt.offsetMax = Vector2(Util.getLiuHaiValue(),0)
                            
--                             baseBg.transform.localScale = Vector2.one
--                             baseBg.transform.localPosition = Vector2.zero
--                             baseBg.name = "__baseBg"
--                             Util.SetSpritAsync("ArtAssets/UIImage/Common","zsy01",baseBg.transform:Find("Image"):GetComponent("Image"))
--                             Util.SetSpritAsync("ArtAssets/UIImage/Common","zsy01",baseBg.transform:Find("Image (1)"):GetComponent("Image"))
--                         end
--                     end)
--                 end
--             end

--             rtran.offsetMin = Vector2(Util.getLiuHaiValue(),0)
--             rtran.offsetMax = Vector2(-Util.getLiuHaiValue(),0)
--         end

--         isIpad = false
--     end

--     return isIpad
 end