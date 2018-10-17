--[[

UILayerInfos  是层级信息

--]]

require "Game/UIFrame/UIBase"
require "Game/UIFrame/UILayer"
require "Game/Modules/Common/layer/MainUILayer"
require "Game/Modules/Common/layer/ViewLayer"
require "Game/Modules/Common/layer/ViewLayerDefault"
-- require "Game/Modules/Common/layer/MaskLayer"
-- require "Game/Modules/Common/layer/TopLayer"

-- require "Game/Modules/Common/view/SystemDesc"

--层级类型  --没有复活层
UILayerType = 
{
    FightTextLayer = "FightTextLayer",   --战斗飘字层
    SceneNameLayer = "SceneNameLayer",   --地图名字层
    AnswerLayer = "AnswerLayer",  --特殊需求要在MainUI下面
    MainUILayer = "MainUILayer", --MainUI层
    PromptLayer = "PromptLayer", --右下角提示框层
    ViewLayer = "ViewLayer",   --UI层
    ViewLayerDefault = "ViewLayerDefault",--UI层2
    MaskLayer = "MaskLayer",  --遮罩层
    AlertLayer = "AlertLayer",  --弹出框层
    GuideLayer = "GuideLayer",  --新手指引
    TopLayer = "TopLayer",    --顶层
    EffectLayer = "EffectLayer", --全局特效层
    TopAlertLayer = "TopAlertLayer",    --顶层
    StoryLayer = "StoryLayer",  --剧情层
    StoryTopLayer = "StoryTopLayer", --剧情夹层
    LoaderLayer = "LoaderLayer", --地图切换加载层
}

--层级信息
UILayerInfos = 
{   
    {name="FightTextLayer",class=UILayer}, --战斗飘字层
    {name="SceneNameLayer",class=UILayer},   --地图名字层
    {name="AnswerLayer",class=UILayer}, --答题特殊层次
    {name="MainUILayer",class=MainUILayer}, --MainUI层
    {name="ReviveLayer",class=UILayer}, --复活层
    {name="PromptLayer",class=UILayer}, --右下角提示框层
    {name="ViewLayer",class=ViewLayer},   --UI层
    {name="ViewLayerDefault",class=ViewLayerDefault},   --UI层
    {name="MaskLayer",class=MaskLayer},  --遮罩层
    {name="AlertLayer",class=UILayer},  --弹出框层
    {name="GuideLayer",class=UILayer},  --新手指引
    {name="TopLayer",class=TopLayer},    --顶层
    {name="EffectLayer",class=UILayer}, --全局特效层
    {name="TopAlertLayer",class=TopAlertLayer}, -- 飘字层
    {name="StoryLayer",class=UILayer},  --剧情层
    {name="StoryTopLayer",class=UILayer},  --剧情层
    {name="LoaderLayer",class=UILayer}, --地图切换加载层
}


--视图ID (界面都放在Prefabs/UI下)
--[[
    path 预制体路径 默认不填用.prefab后缀
    isDontDestroy 是否不销毁 默认false
    layer 显示层级 默认ViewLayer
    atlas 界面图集
    fullScreen 是否全屏界面 
    1--全屏界面带截屏，有4:3下缩放规则，关闭会回收gc  
    2--全屏界面不截屏，有4:3下缩放规则，关闭会回收gc
    3--不是全屏界面，但需要当它是全屏界面，因为关闭需要回收gc   
]]
UIViewID = 
{
    TestUI = {path="Module/Test/TestUI",layer="ViewLayer"},
    CommonAlert = { path="Common/CommonAlert",isDontDestroy=true,layer="TopAlertLayer" },
}


--模块相关   
require "Game/Managers/ModuleManager"
require "Game/Data/moduleConfig"
require "Game/Modules/BaseModule"



-- ui资源 关系 用于回收内存   界面相关
require "Game/UIFrame/UIManager"
UIViewAssets = require "Game/Data/UIViewAssets"
UIViewPath = require "Game/Data/UIViewPath"




--后加的，感觉功能和Util重复了
require "Game/Common/UIUtils/UIUtils"


--公共的ui界面