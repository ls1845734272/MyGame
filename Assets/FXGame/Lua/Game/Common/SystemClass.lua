



---------------------------------------table表-------------------------
--通过值检索是否存在
--list 即table表
--需要检索的value 可为table
function table.include(list , value)
    if(not list)then return false end;
    for k,v in pairs(list) do
        if v==value then 
            return true
        end
    end
    return false
end


--通过值检索是否存在
--list 即table表
--需要检索的value 可为table
function table.indexOf(list , value)
    for k,v in pairs(list) do
        if v==value then 
            return k
        end
    end
    return nil
end



function table.containKey( t, key )
    for k, v in pairs(t) do
        if key == k then
            return true;
        end
    end
    return false;
end
----------------------------------string----------------------

--字符分割
function string.split(str, delimiter)
    if str==nil or str=='' or delimiter==nil then
        return nil
    end
    
    local result = {}
    for match in (str..delimiter):gmatch("(.-)"..delimiter) do
        table.insert(result, match)
    end
    return result
end


-- 把[a,b,c,d ...][1000,3,...] 转成table {{a,b,c,d ...},{1000,3 ...}}
function string.formatToArray(str)
  local r = {}
  if str and #str > 0 then
    for k in string.gmatch(str,"%b[]") do
      k=string.sub(k,2,-2)
      table.insert(r,string.split(k,","))
    end
  end
  return r
end


function string.strSplit(str,delimeter)  
    local find, sub, insert = string.find, string.sub, table.insert  
    local res = {}  
    local start, start_pos, end_pos = 1, 1, 1  
    while true do  
        start_pos, end_pos = find(str, delimeter, start, true)  
        if not start_pos then  
            break  
        end  
        insert(res, sub(str, start, start_pos - 1))  
        start = end_pos + 1    
    end  
    insert(res, sub(str,start))  
    return res  
end 

-- 去除字符串两边的空格  
function string.trim(s)   
    return (string.gsub(s, "^%s*(.-)%s*$", "%1"))  
end  

