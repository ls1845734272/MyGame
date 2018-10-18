using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Reflection;
using LuaInterface;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FXGame
{
    public class Util
    {

        public static string FormatException(System.Exception e)
        {
            string strSource = string.IsNullOrEmpty(e.Source) ? "<no source>" : e.Source.Substring(0, e.Source.Length - 2);
            return string.Format("{0}\nLua (at {2})", e.Message, string.Empty, strSource);
        }

        /// <summary>
        /// 获得打包的文件夹
        /// </summary>
        public static string DataPath2
        {

            get
            {
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    // int i = Application.dataPath.LastIndexOf('/');
                    // return Application.dataPath.Substring(0, i + 1) + game + "/";
                    return "myfxgame_xl/";
                }
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    return "D:/myfxgame_xl/";
                }

                return "myfxgame_xl";

            }
        }

        /// <summary>
        /// 程序清单文件
        /// </summary>
        /// <returns></returns>
        public static string AppUpdateFile
        {
            get { return "update" + AppConst.ExtName; }
        }

        public static string AppDllFile
        {
            get { return "Assembly-CSharp.dll"; }
        }

        public static string md5file(byte[] bt)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(bt);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string md5file(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();



                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        //复制到剪切版
        public static void CopyClipboard(string value)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            using (AndroidJavaObject currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
            {
                string clipboard = new AndroidJavaClass("android.content.Context").GetStatic<string>("CLIPBOARD_SERVICE");
                using (AndroidJavaObject clipboardManager = currentActivity.Call<AndroidJavaObject>("getSystemService", clipboard))
                {
                    clipboardManager.Call("setText", value);
                }
            }
#elif UNITY_IOS

#endif
        }



    }
}
