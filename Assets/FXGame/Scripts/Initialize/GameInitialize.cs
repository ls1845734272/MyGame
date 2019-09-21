using UnityEngine;
using System.Collections;
using LuaInterface;
using System;
using System.IO;
using UnityEngine.EventSystems;

using System.Collections.Generic;
namespace FXGame
{
    public class GameInitialize : MonoBehaviour
    {
        public void Awake()
        {
            FXGame.Managers.LuaManager.Instance.TestStart();
        }
    }


    //public class GameInitialize : MonoBehaviour
    //{
    //    [Tooltip("调试模式下，资源及lua脚本将从开发项目中获取，否则为对应平台的StreamingAssets中获取")]
    //    public bool m_isDebug = true;
    //    [Tooltip("更新模式：开启此模式资源及脚本将从服务器下载。")]
    //    public bool m_isUpdate = false;
    //    [Tooltip("是否开启控制台")]
    //    public bool isUseConsole = true;

    //    private bool m_IsInit = false;

    //    private UpdateController uc;
    //    public void Awake()
    //    {
    //        UpdateController.time = Time.realtimeSinceStartup;
    //        Application.targetFrameRate = AppConst.GameFrameRate;//设置游戏帧频率
    //        Screen.sleepTimeout = SleepTimeout.NeverSleep;//当前应用不休眠

    //        gameObject.name = AppConst.AppName;
    //        DontDestroyOnLoad(this);

    //        gameObject.AddComponent<UpdateInternalController>();
    //    }
    //}

}



