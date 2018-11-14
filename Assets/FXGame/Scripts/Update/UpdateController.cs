using UnityEngine;
using FXGame.Managers;
using System.Timers;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace FXGame
{
    public enum BugType
    {
        Lua_Error = 1,// 1lua报错
        ResUpdate_Error = 2,// 2部分资源更新失败
        GetServerList_Error = 3,// 3获取服务器列表失败
        LoginSDK_Error = 4,// 4登陆SDK失败
        Login_Error = 5,// 5登陆游戏登陆服失败
        LoginGame_Error = 6,// 6登陆游戏游戏服失败
        CreateRole_Error = 7,// 7创号失败
        NoRoleInfo_Error = 8,// 8进地图长时间未收到主角信息下推
        UnWalk_Error = 9,// 9玩家走到一个不可行走区
        GetGoodList_Error = 10,// 10获取充值商品列表失败
        ChannelId_Error = 11,// 11没有找到channel_Id
        ServerBug_Error = 100,// 100服务端Bug监控信息
    }
    public class UpdateController
    {
        public static float time = 0;
    }
}