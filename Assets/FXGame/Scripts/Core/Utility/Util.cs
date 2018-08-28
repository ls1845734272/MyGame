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
    }
}
