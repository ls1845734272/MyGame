using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FXGame;
using UnityEditor;
using UnityEngine;

public class BuildLua : ScriptableObject
{
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();
    public static BuildTarget target;

    [MenuItem("FXGame/Build/Build Lua Windows",false,10003)]
    static void Build_Lua_Windows()
    {
        //System.Diagnostics.Debugger.Log(1, "dd", "11111111111");
        UnityEngine.Debug.Log("FXGame/Build/Build Lua Windows");
        //target = BuildTarget.StandaloneWindows;
        //BuildLuaScript();
    }
    public static void BuildLuaScript()
    {
        //这是build资源的路径
        string buildPath = BuildUtil.GetBuildDir(target);

        string resPath = Path.Combine(buildPath, "lua");

        if (Directory.Exists(resPath))
        {
            Directory.Delete(resPath, true);
        }
        AssetDatabase.Refresh();

        ClearAllLuaFiles();
        if (AppConst.LuaBundleMode)
        {
            //HandleLuaBundle();
        }
        else
        {
            HandleLuaFile(resPath);
        }
        BuildUtil.GenVersionFile(buildPath, BuildUtil.VersionEnum.LUA);
        BuildUtil.BuildFileIndex(buildPath);
        AssetDatabase.Refresh();
    }


    /// <summary>
    /// 处理Lua文件
    /// </summary>
    static void HandleLuaFile(string respath)
    {
        //string luaPath = BuildUtil.AppDataPath + "/StreamingAssets/" + BuildUtil.GetBuildOsDir(target) + "/lua/";

        //----------复制Lua文件----------------
        if (!Directory.Exists(respath))
        {
            Directory.CreateDirectory(respath);
        }


        string[] luaPaths = { AppConst.APPRoot + "/lua/",
                              AppConst.APPRoot + "/Tolua/Lua/" };

        for (int i = 0; i < luaPaths.Length; i++)
        {
            paths.Clear(); files.Clear();
            string luaDataPath = luaPaths[i].ToLower();
            BuildUtil.Recursive(luaDataPath, files);
            int n = 0;
            foreach (string f in files)
            {
                if (f.EndsWith(".meta")) continue;
                string newfile = f.Replace(luaDataPath, "");
                string newpath = Path.Combine(respath, newfile);
                string path = Path.GetDirectoryName(newpath);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
                if (AppConst.LuaByteMode)
                {
                    EncodeLuaFile(f, newpath);
                }
                else
                {
                    File.Copy(f, newpath, true);
                }
                UpdateProgress(n++, files.Count, newpath);
            }
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static void ClearAllLuaFiles()
    {
        string osName = BuildUtil.GetBuildOsDir(target);

        string osPath = Application.streamingAssetsPath + "/" + osName;

        string buildPath = BuildUtil.GetBuildDir(target);

        string resPath = Path.Combine(buildPath, "/lua");

        if (Directory.Exists(osPath))
        {
            string[] files = Directory.GetFiles(osPath, "Lua*.unity3d");

            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }
        }

        if (Directory.Exists(resPath))
        {
            Directory.Delete(resPath, true);
        }

        string path = osPath + "/Lua";

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        path = Application.dataPath + "/Resources/Lua";

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        path = FXGame.AppConst.GetAppDownDir() + "/" + osName + "/Lua";

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }


    static void UpdateProgress(int progress, int progressMax, string desc)
    {
        string title = "Processing...[" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }


    public static void EncodeLuaFile(string srcFile, string outFile)
    {
        if (!srcFile.ToLower().EndsWith(".lua"))
        {
            File.Copy(srcFile, outFile, true);
            return;
        }
        bool isWin = true;
        string luaexe = string.Empty;
        string args = string.Empty;
        string exedir = string.Empty;
        string currDir = Directory.GetCurrentDirectory();
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            isWin = true;
            luaexe = "luajit.exe";
            args = "-b " + srcFile + " " + outFile;
            exedir = BuildUtil.AppDataPath.Replace("assets", "") + "LuaEncoder/luajit/";
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            isWin = false;
            luaexe = "./luac";
            args = "-o " + outFile + " " + srcFile;
            exedir = BuildUtil.AppDataPath.Replace("assets", "") + "LuaEncoder/luavm/";
        }
        Directory.SetCurrentDirectory(exedir);
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = luaexe;
        info.Arguments = args;
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = isWin;
        info.ErrorDialog = true;
        //Util.Log(info.FileName + " " + info.Arguments);

        Process pro = Process.Start(info);
        pro.WaitForExit();
        Directory.SetCurrentDirectory(currDir);
    }

}
