using UnityEngine;
using UnityEditor;
using System.IO;
using FXGame;
using System.Collections.Generic;
using LuaInterface;

public class BuildUtil {

    public enum VersionEnum
    {
        NONE,
        APP,
        LUA, RES
    }

    public enum PackageType
    {
        separate,
        together,
        skip,
    }

    /// <summary>
    /// 下载优先级  --这是在updateManager.cs脚本中
    /// </summary>
    public enum DownLoadLevel
    {
        FirstPack = 0,//首包
        Highest = 1,//最高
        Guilde = 2,//指引
        Middle = 3,//中
        Low = 4//低
    }

    /// <summary>
    /// 生成迭代版本文件
    /// </summary>
    /// <param name="path"></param>
    public static GameVersion GenVersionFile(string path, VersionEnum verType, GameVersion ver = null)
    {
        string filePath = path + "/version.txt";
        GameVersion gver = null;
        if (null != ver)
        {
            gver = ver;
        }
        else
        {
            string oldvertxt = "";
            gver = new GameVersion();
            if (File.Exists(filePath))
            {
                oldvertxt = File.ReadAllText(filePath);
                File.Delete(filePath);
            }
            gver.SetVersionTxt(oldvertxt);
        }

        if (verType == VersionEnum.APP)
            gver.IterateAppVer();
        else if (verType == VersionEnum.LUA)
            gver.IterateLuaVer();
        else if (verType == VersionEnum.RES)
            gver.IterateResVer();

        //获取服务器版本
        LuaState lua = new LuaState();
        lua.Start();
        lua.DoFile("Game/Data/GameConfig.lua");
        LuaFunction func = lua.GetFunction("GameConfig.getServerVersion");
        if (func != null)
        {
            //string objs = func.Call();
            //if (objs != null)
            //{
            //gver.ServerVersion = new System.Version(objs[0].ToString());
            //}
            gver.ServerVersion = new System.Version("0.0.0.1");
        }

        string verContent = gver.GetVersionTxt();

        //Debug.Log("Build Version : \n" + verContent);
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        File.WriteAllText(filePath, verContent);
        return gver;
    }

    static GameAssetsManifest abm = null;
    /// <summary>
    /// 生成文件索引列表信息
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isSperate"></param>
    /// <param name="target"></param>
    public static void BuildFileIndex(string path, bool isSperate = false, BuildTarget target = BuildTarget.Android)
    {
        Dictionary<string, int> firstMap = new Dictionary<string, int>();
        Dictionary<string, int> FrontMap = new Dictionary<string, int>();
        Dictionary<string, int> GuildeMap = new Dictionary<string, int>();
        if (isSperate)
        {
            string[] lines = File.ReadAllLines(Util.DataPath2 + "/first.txt");
            string baseDir = Util.DataPath2 + GetBuildOsDir(target) + "/";
            foreach (var item in lines)
            {
                string resName = item.Replace("//", "/").Replace(baseDir, "").ToLower();
                firstMap[resName] = 0;
            }

            lines = File.ReadAllLines(Util.DataPath2 + "/guildeLoad.txt");
            int o = 1;
            foreach (var l in lines)
            {
                GuildeMap["res/" + l] = o++;
            }
            o = 1;
            lines = File.ReadAllLines(Util.DataPath2 + "/frontLoad.txt");
            foreach (var li in lines)
            {
                FrontMap["res/" + li] = o++;
            }

        }

        string resPath = path.Replace("\\", "/");
        ///----------------------创建文件列表-----------------------
        string newFilePath = resPath + "/files.txt";
        if (File.Exists(newFilePath)) File.Delete(newFilePath);

        List<string> files = new List<string>();

        Recursive(resPath, files);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Count; i++)
        {
            string file = files[i];
            long size = new FileInfo(file).Length;
            //string ext = Path.GetExtension(file);
            if (file.EndsWith(".meta") || file.Contains(".DS_Store")
                || file.EndsWith(".manifest") || file.Contains("version.txt")) continue;

            string lpath = file.Substring(resPath.Length + 1);

            string md5 = "";
            string value = "";
            string md5_read = "";
            if (lpath == FXGame.Util.AppUpdateFile || lpath == "server_res_list_md5.txt")
            {
                continue;
            }

            int downloadLevel = (int)DownLoadLevel.Low;
            int order = 0;
            int resType = 0;
            string fname = Path.GetFileNameWithoutExtension(file);//

            if (lpath.StartsWith("res") && lpath.Contains(AppConst.ExtName) && fname != "manifest")
            {
                string abmPath = "";
                if (null == abm)
                {
                    abmPath = path + "/res/manifest" + AppConst.ExtName;
                    abmPath = abmPath.Replace("\\", "/");
                    if (!File.Exists(abmPath)) continue;
                    AssetBundle ab = AssetBundle.LoadFromFile(abmPath);
                    UnityEngine.Object[] names = ab.LoadAllAssets();
                    foreach (var item in names)
                    {
                        Debug.Log(item.name);
                    }
                    abm = ab.LoadAsset<GameAssetsManifest>("GameAssetsManifest");
                    ab.Unload(false);
                }

                string bname = lpath.Substring(4);
                md5 = abm.GetAssetBundleHash(bname).ToString();
            }
            else
            {
                md5 = Util.md5file(file);
                md5_read = md5;
            }

            if (string.IsNullOrEmpty(md5))
            {
                continue;
            }
            else
            {
                md5_read = Util.md5file(file);
            }

            value = file.Replace(resPath, string.Empty).Substring(1);
            value = file.Replace(resPath, string.Empty).Substring(1);

            int tmp = 0;
            if (isSperate && GuildeMap.TryGetValue(value, out order))
            {
                downloadLevel = (int)DownLoadLevel.Middle;
                order = tmp;
            }
            if (FrontMap.TryGetValue(value, out tmp))
            {
                downloadLevel = (int)DownLoadLevel.Highest;
                order = tmp;
            }

            if (!isSperate || value.StartsWith("lua/lua") ||
                (firstMap != null && firstMap.TryGetValue(value, out tmp)))
            {
                downloadLevel = 0;
                order = tmp;
            }
            if (value == Util.AppDllFile)
            {
                downloadLevel = 0;
            }
            sw.WriteLine(value + "|" + md5 + "|" + size + "|" + resType + "|" + downloadLevel + "|" + order + "|" + md5_read);
        }
        abm = null;
        sw.Close(); fs.Close();
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    public static void Recursive(string path, List<string> files)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            //paths.Add(dir.Replace('\\', '/'));
            Recursive(dir, files);
        }
    }

    /// <summary>
    /// 数据目录
    /// </summary>
    public static string AppDataPath
    {
        get { return Application.dataPath.ToLower(); }
    }

    public static string GetBuildOsDir(BuildTarget target)
    {
        if (target == BuildTarget.StandaloneWindows)
            return "win";
        else if (target == BuildTarget.Android)
            return "android";
        else if (target == BuildTarget.iOS)
            return "ios";
        return "temp";
    }

    public static string GetBuildDir(BuildTarget target)
    {
        string dir = "";
        if (target == BuildTarget.StandaloneWindows)
        {
            dir = Path.Combine(Application.dataPath, "../AssetBundles");
        }
        else if (target == BuildTarget.Android)
        {
            dir = @"D:\myfxgame_xl";
        }
        else if (target == BuildTarget.iOS)
        {
            dir = "/Users/Shared/myfxgame";
        }
        return Path.Combine(dir, BuildUtil.GetBuildOsDir(target));
    }
}
