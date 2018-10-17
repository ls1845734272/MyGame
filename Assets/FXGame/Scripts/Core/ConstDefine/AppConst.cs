using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FXGame
{
    public class AppConst 
    {
        public static bool DebugMode = true;                        //调试模式-用于内部测试

		public static bool VerifyApp = false;       //是否是审核状态p

		public static bool forceLowQuality = false;    //强制最低画质，不可切换高画质 低配手机

        public const string AppName = "FXGame";                     //应用程序名称
    }

}
