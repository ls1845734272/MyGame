local TestUIProxy = class(Proxy,"TestUIProxy")


function TestUIProxy:Exception( what,code )
   print("错误")
end

return TestUIProxy