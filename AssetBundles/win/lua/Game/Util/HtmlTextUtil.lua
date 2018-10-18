HtmlTextUtil = HtmlTextUtil or {}

function HtmlTextUtil.addColor(str,colorStr)
	if(not str )then return "" end;
	if(not colorStr)then return str end;
	return "<color=" .. colorStr..">" .. str .. "</color>"
end

function HtmlTextUtil.addBold(str)
	return "<b>" .. str .. "</b>"
end

function HtmlTextUtil.addUnderline(str)
  return "<u>"..str.."</u>"
end


--------------------------------
-- 格式化字符串成html样式
-- @param		#string		str			原字符串
-- @param		#string		color		颜色
-- @param		#int		size		字号
-- @param		#int		strokeWidth	描边粗细，为0时不描边
-- @param		#boolean	underline	是否带下划线
-- @return	#string		格式化成带html格式的字符串
function HtmlTextUtil.formatString(str, color, size, strokeWidth, underline) 
	local attr = ""
	if color then
		attr = attr..string.format(" color='%s'", color)
	end
	if size then
		attr = attr..string.format(" size='%d'", size)
	end
	if strokeWidth then
		attr = attr..string.format(" strokeWidth='%d'", strokeWidth)
	end
	if underline then
		return string.format("<font%s><u>%s</u></font>", attr, str)
	end
	return string.format("<font%s>%s</font>", attr, str)
end

--------------------------------
-- 格式化字符串描边
-- @param		#string		str			原字符串
-- @param		#int		strokeWidth	描边粗细，为0时不描边
-- @return	#string		格式化成带html格式的字符串
function HtmlTextUtil.strokeWidthString(str, strokeWidth) 
	local attr = ""			
	strokeWidth = strokeWidth or 1	
	attr = attr..string.format(" strokeWidth='%d'", strokeWidth)
	return string.format("<font%s>%s</font>", attr, str)
end

return HtmlTextUtil;