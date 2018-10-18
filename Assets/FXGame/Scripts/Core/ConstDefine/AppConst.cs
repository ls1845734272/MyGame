using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FXGame
{
    public class AppConst 
    {
        public static bool DebugMode = true;                        //调试模式-用于内部测试
        public static bool LuaByteMode = false;                     //Lua字节码模式-默认关闭 
        public static bool LuaBundleMode = false;                   //Lua代码AssetBundle模式-默认关闭 

        public static bool VerifyApp = false;       //是否是审核状态p

		public static bool forceLowQuality = false;    //强制最低画质，不可切换高画质 低配手机

        public const string AppName = "FXGame";                     //应用程序名称
        public const string ExtName = ".png";                     //资源扩展名

        public static string APPRoot
        {
            get { return Application.dataPath + "/" + AppName; }
        }

        public static string GetAppDownDir()
        {
#if UNITY_ANDROID
              return Application.persistentDataPath;
#elif UNITY_IOS
              return Application.temporaryCachePath;
#else
            return "";
#endif

        }
    }


}
