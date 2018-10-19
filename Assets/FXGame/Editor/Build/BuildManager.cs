using UnityEngine;
using UnityEditor;
using System.IO;
using FXGame;
using System.Collections.Generic;
using System;

public class BuildManager
{
    public enum BuildType
    {
        Dir,//目录打包
        Dir_ROOT,//目录根打包
        Dir_LAST,//目录最下层目录打包
        Separate,//分离打包
        Ignore, //忽略目录
        Model,//模型打包
        ModelPrefab,//模型打包
        TextureMat//材质和贴图合并打包
    }

    private static Dictionary<string, BuildType> dirConfig = new Dictionary<string, BuildType>() {
        //{ "Shaders",BuildType.Dir },

        { "Prefabs/UI",BuildType.Dir_LAST },

        { "ArtAssets/UI",BuildType.Dir},
        { "ArtAssets/UIImage",BuildType.Dir_LAST},

    };

    /// <summary>
    /// 获得构建包系统
    /// </summary>
    /// <returns></returns>
    public static List<AssetBundleBuild> GetBuildAll()
    {
        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
        foreach (var item in dirConfig)
        {
            ConstructABs(Application.dataPath + "/" + item.Key, buildList, item.Value);
        }
        return buildList;
    }


    private static Dictionary<string, bool> hasAB = new Dictionary<string, bool>();
    /// <summary>
    /// 构建AB包系统
    /// </summary>
    /// <param name="path"></param>
    /// <param name="buildList"></param>
    /// <param name="packageType"></param>
    public static void ConstructABs(string path, List<AssetBundleBuild> buildList, BuildType buildType)
    {

        path = path.Replace('\\', '/');
        //path = F:/ MyGame / Assets / Prefabs / UI
        BuildType _buildType = buildType;
        string bundleDir = path.Substring(Application.dataPath.Length + 1).ToLower().Replace("\\", "/");
        //bundleDir = prefabs / ui


        List<string> files = new List<string>();
        //打包种类处理
        switch (_buildType)
        {
            case BuildType.Dir://当前目录下的文件都打包在一起 包名就是目录名
                {
                    files.Clear();
                    findAllFiles(path, files);
                    if (files.Count == 0) break;

                    createAssetBundle(bundleDir + AppConst.ExtName, files.ToArray(), buildList);
                    break;
                }
            case BuildType.Dir_ROOT://以当前目录的下 最近那层目录打包
                {
                    string[] dirRoots = Directory.GetDirectories(path);
                    foreach (var itemRoot in dirRoots)
                    {
                        ConstructABs(itemRoot, buildList, BuildType.Dir);
                    }
                    files.Clear();
                    findAllFiles(path, files, false);
                    if (files.Count == 0) break;
                    createAssetBundle(bundleDir + AppConst.ExtName, files.ToArray(), buildList);
                    break;
                }
            case BuildType.Dir_LAST://查找到最底层的目录去打包
                {
                    string[] dirs = Directory.GetDirectories(path);
                    foreach (var lasrDir in dirs)
                    {
                        ConstructABs(lasrDir, buildList, BuildType.Dir_LAST);
                    }
                    files.Clear();
                    findAllFiles(path, files, false);
                    if (files.Count == 0) break;
                    createAssetBundle(bundleDir + AppConst.ExtName, files.ToArray(), buildList);
                    break;
                }
            case BuildType.Separate://分离单文件打包
                {
                    string[] cdirs = Directory.GetDirectories(path);
                    foreach (var cdir in cdirs)
                    {
                        ConstructABs(cdir, buildList, BuildType.Separate);
                    }
                    string[] sfiles = Directory.GetFiles(path);
                    for (int i = 0; i < sfiles.Length; i++)
                    {
                        string fpath = sfiles[i];
                        if (fpath.Contains(".meta") || fpath.Contains(".cs") || fpath.Contains(".unity")) continue;
                        fpath = fpath.Substring(Application.dataPath.Length - 6).Replace("\\", "/");

                        createAssetBundle(bundleDir + "/" + Path.GetFileNameWithoutExtension(fpath).ToLower() + AppConst.ExtName,
                                new string[] { fpath }, buildList);
                    }
                    break;
                }
            case BuildType.Model:
                {
                    string[] dirRoots = Directory.GetDirectories(path);

                    string prefab = "";
                    string abname = "";
                    foreach (var itemRoot in dirRoots)
                    {
                        files.Clear();
                        findAllFiles(itemRoot, files);
                        if (files.Count == 0) continue;
                        prefab = itemRoot.Replace("ArtAssets/Characters", "Prefabs/Characters") + "_tpose.prefab";
                        prefab = prefab.Substring(Application.dataPath.Length - 6).Replace("\\", "/");
                        files.Add(prefab);
                        hasAB[prefab] = true;

                        abname = itemRoot.Substring(Application.dataPath.Length + 1).ToLower().Replace("\\", "/");
                        createAssetBundle(abname + AppConst.ExtName, files.ToArray(), buildList);
                        //ConstructABs(itemRoot, buildList, BuildType.Dir);
                    }

                    string prefabDir = path.Replace("ArtAssets/Characters", "Prefabs/Characters");
                    ConstructABs(prefabDir, buildList, BuildType.ModelPrefab);

                    files.Clear();
                    findAllFiles(path, files, false);
                    if (files.Count == 0) break;
                    createAssetBundle(bundleDir + AppConst.ExtName, files.ToArray(), buildList);
                    break;
                }
            case BuildType.ModelPrefab:
                {
                    string[] cdirs = Directory.GetDirectories(path);
                    foreach (var cdir in cdirs)
                    {
                        ConstructABs(cdir, buildList, BuildType.Separate);
                    }
                    string[] sfiles = Directory.GetFiles(path);
                    bool isBuilded = false;
                    for (int i = 0; i < sfiles.Length; i++)
                    {
                        string fpath = sfiles[i];
                        if (fpath.Contains(".meta") || fpath.Contains(".cs") || fpath.Contains(".unity")) continue;
                        fpath = fpath.Substring(Application.dataPath.Length - 6).Replace("\\", "/");
                        if (hasAB.TryGetValue(fpath, out isBuilded))
                        {
                            continue;
                        }
                        createAssetBundle(bundleDir + "/" + Path.GetFileNameWithoutExtension(fpath).ToLower() + AppConst.ExtName,
                                new string[] { fpath }, buildList);
                    }
                    break;
                }
            case BuildType.TextureMat://材质和贴图合并打包
                {
                    string matPath = Application.dataPath + "/ArtAssets/Environment/Materials";
                    string matPath1 = Application.dataPath + "/ArtAssets/Environment/Materials1";

                    string[] files0 = Directory.GetFiles(matPath);
                    string[] files1 = Directory.GetFiles(matPath1);

                    string[] mats = new string[files0.Length + files1.Length];
                    Array.Copy(files0, 0, mats, 0, files0.Length);
                    Array.Copy(files1, 0, mats, files0.Length, files1.Length);

                    Dictionary<string, List<string>> fileMap = new Dictionary<string, List<string>>();
                    List<string> abs = null;
                    string fpath = "";
                    string imgname;
                    foreach (var mat in mats)
                    {
                        if (mat.EndsWith(".meta")) continue;
                        fpath = mat.Substring(Application.dataPath.Length - 6).Replace("\\", "/");
                        string[] deps = AssetDatabase.GetDependencies(fpath);
                        if (deps.Length > 2)
                        {
                            imgname = Path.GetDirectoryName(mat.Substring(Application.dataPath.Length + 1).ToLower().Replace("\\", "/")) + "/" + Path.GetFileNameWithoutExtension(fpath) + AppConst.ExtName;
                            // createAssetBundle(imgname, new string[] { fpath}, buildList);
                        }
                        else
                        {
                            foreach (var name in deps)
                            {
                                imgname = Path.GetFileNameWithoutExtension(name);
                                if (fileMap.TryGetValue(imgname, out abs))
                                {
                                    if (!abs.Contains(fpath))
                                    {
                                        abs.Add(fpath);
                                    }
                                }
                                else
                                {
                                    fileMap[imgname] = new List<string>() { fpath };
                                }
                            }
                        }
                    }

                    string tpath = Application.dataPath + "/ArtAssets/Environment/Textures";
                    string[] tfiles = Directory.GetFiles(tpath);
                    //第一步 确立关键字 并找到关键字对应的3张贴图 
                    string keyName = "";
                    Dictionary<string, List<string>> keyDict = new Dictionary<string, List<string>>();
                    List<string> abfiles = null;

                    foreach (var file in tfiles)
                    {
                        if (file.EndsWith(".meta")) continue;
                        fpath = file.Substring(Application.dataPath.Length - 6).Replace("\\", "/");
                        keyName = Path.GetFileNameWithoutExtension(fpath);
                        if (keyName.EndsWith("_specular") || keyName.EndsWith("_normal"))
                        {
                            keyName = keyName.Replace("_specular", "").Replace("_normal", "");
                            if (keyDict.TryGetValue(keyName, out abfiles))
                            {
                                if (!abfiles.Contains(fpath))
                                {
                                    abfiles.Add(fpath);
                                }
                            }
                            else
                            {
                                keyDict[keyName] = new List<string>() { fpath };
                            }
                        }
                        else
                        {
                            keyDict[keyName] = new List<string>() { fpath };
                        }
                    }

                    List<string> nameList = null;
                    bundleDir = tpath.Substring(Application.dataPath.Length + 1).ToLower().Replace("\\", "/");
                    foreach (var item in keyDict)
                    {
                        nameList = item.Value;
                        int len = nameList.Count;
                        for (int i = 0; i < len; i++)
                        {
                            string s = nameList[i];
                            imgname = Path.GetFileNameWithoutExtension(s);
                            // if (fileMap.TryGetValue(imgname, out abs))
                            // {
                            //     foreach (var ass in abs)
                            //     {
                            //         if (!nameList.Contains(ass)) nameList.Add(ass);
                            //     }
                            // }
                        }
                        createAssetBundle(bundleDir + "/" + item.Key.ToLower() + AppConst.ExtName, nameList.ToArray(), buildList);
                        //AssetBundleBuild ab = new AssetBundleBuild();
                        //ab.assetBundleName = "artassets/environment/" + item.Key + AppConst.ExtName;
                        //ab.assetNames = nameList.ToArray();
                    }
                    break;
                }
            case BuildType.Ignore://忽略的目录
                {
                    break;
                }
        }

        hasAB.Clear();
    }


    /// <summary>
    /// 文件和包对应表
    /// </summary>
    public static Dictionary<string, string> idxMap = new Dictionary<string, string>();


    /// <summary>
    /// 创建AB包
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="assets"></param>
    /// <param name="buildList"></param>
    /// <returns></returns>
    static AssetBundleBuild createAssetBundle(string abName, string[] assets, List<AssetBundleBuild> buildList)
    {

        //abName = artassets / ui.png   Dir

        AssetBundleBuild ab = new AssetBundleBuild();
        ab.assetBundleName = abName;
        ab.assetNames = assets;
        foreach (var asset in assets)
        {
            //File.AppendAllText(Util.DataPath + "/fileMap.txt", asset + "," + abName + "\n");

            // asset =  Assets / ArtAssets / UI / FirstCharge.prefab   Dir
            idxMap[asset] = abName;
        }
        buildList.Add(ab);
        return ab;
    }


    public static void findAllFiles(string dir, List<string> filelist, bool isChildDir = true)
    {
        string[] files = Directory.GetFiles(dir);
        string fpath = "";
        foreach (var f in files)
        {
            //f   =   F:/MyGame/Assets/ArtAssets/UI\FirstCharge.prefab
            if (f.Contains(".meta") || f.Contains(".cs") || f.Contains(".unity")) continue;
            fpath = f.Substring(Application.dataPath.Length - 6).Replace("\\", "/");
            //fpath = Assets / ArtAssets / UI / FirstCharge.prefab
            filelist.Add(fpath);
        }

        if (isChildDir)
        {
            string[] dirs = Directory.GetDirectories(dir);
            foreach (var item in dirs)
            {
                //item  =  F:/ MyGame / Assets / ArtAssets / UI\Materials
                findAllFiles(item, filelist);
            }
        }
    }
}
