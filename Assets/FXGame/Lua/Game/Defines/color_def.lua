ColorType = ColorType or {} 

function toColor(hex) 
    local red = string.sub(hex, 1, 2)  
    local green = string.sub(hex, 3, 4)  
    local blue = string.sub(hex, 5, 6)
    local alpha = string.sub(hex,7,8)

    alpha = alpha=="" and "FF" or alpha --默认不透明

    red = tonumber(red, 16) / 255  
    green = tonumber(green, 16) / 255  
    blue = tonumber(blue, 16) / 255
    alpha =  tonumber(alpha, 16) / 255
    local _color = Color.New(red, green, blue,alpha)
    _color.hex = '#'..hex
    return _color
end 

ColorType.none = toColor("FFFFFF")
--(黑色底)道具颜色 白 绿 蓝 紫 橙 红 粉红 金 彩
ColorType.white = toColor("f2f1e9")
ColorType.green = toColor("4aff11")
ColorType.blue = toColor("4da5ff")
ColorType.purple = toColor("d05eff")
ColorType.orange = toColor("ff8e00")
ColorType.red = toColor("FF4949")
ColorType.pink = toColor("fe57ec")
ColorType.golden = toColor("ffee33")
ColorType.rainbow = toColor("fd568a")


--白色底的色值
ColorType.itemWhiteColor = {
    toColor("7f7868"),
    toColor("009219"),
    toColor("0065ac"),
    toColor("9500b0"),
    toColor("c35a01"),
    toColor("da0000"),
    toColor("fc15b8"),
    toColor("ffee33"),
}

function ColorType.getItemColor(color)
    return ColorType.itemColor[color] or ColorType.none
end   

--颜色类型
ColorType.itemColor = { 
    ColorType.white , 
    ColorType.green , 
    ColorType.blue , 
    ColorType.purple ,
    ColorType.orange ,
    ColorType.red , 
    ColorType.pink,
    ColorType.golden,
    ColorType.rainbow,
}

-- //颜色
-- enum EColor
-- {
--     EColorWhite			= 1, //白色		
--     EColorGreen			= 2, //绿色		
--     EColorBlue			= 3, //蓝色		
--     EColorPurple		= 4, //紫色		
--     EColorOrange		= 5, //橙色
--     EColorRed			= 6, //红色
--     EColorGolden 		= 7, //粉色
--     EColorYellow		= 8, //黄色
--     EColorClours		= 9, //彩色
-- };

--白底色值
function ColorType.getItemStrWhiteColor(color)
    if(color == 1) then
        return "7f7868"
    elseif(color == 2) then
        return "009219"
    elseif(color == 3) then
        return "0065ac"
    elseif(color == 4) then
        return "9500b0"
    elseif(color == 5) then
        return "c35a01"
    elseif(color == 6) then
        return "da0000"
    elseif(color == 7) then
        return "fc15b8"
    end
    return "39301e"
end

--黑底色值
function ColorType.getItemStrBackColor(color)
    if(color == 1) then
        return "f2f1e9"
    elseif(color == 2) then
        return "4aff11"
    elseif(color == 3) then
        return "00c6ff"
    elseif(color == 4) then
        return "d05eff"
    elseif(color == 5) then
        return "ff8e00"
    elseif(color == 6) then
        return "ff0023"
    elseif(color == 7) then
        return "fe57ec"
    end
    return "ffffff"
end