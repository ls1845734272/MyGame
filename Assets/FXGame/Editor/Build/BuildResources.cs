using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class BuildResources : ScriptableObject
{
    static BuildTarget target;

#if UNITY_ANDROID
        static BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.IgnoreTypeTreeChanges;
        // static BuildAssetBundleOptions options = BuildAssetBundleOptions.None | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.IgnoreTypeTreeChanges;
#elif UNITY_IOS
        // static BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.IgnoreTypeTreeChanges;
        static BuildAssetBundleOptions options = BuildAssetBundleOptions.None | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.IgnoreTypeTreeChanges;
#else
    static BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.IgnoreTypeTreeChanges;
#endif


    [MenuItem("FXGame/Build/Build Resources Windows", false, 10053)]
    static void Build_Resources_Windows()
    {
        target = BuildTarget.StandaloneWindows;
        BuildResourcesExecute();
    }

    [MenuItem("FXGame/Build/Build Lua And Resources Windows", false, 10103)]
    static void Build_LuaAndResources_Windows()
    {
        target = BuildTarget.StandaloneWindows;
        Build_LuaAndResources_Execute();
    }

    static void Build_LuaAndResources_Execute()
    {
        BuildLua.target = target;
        BuildLua.BuildLuaScript();
        BuildResourcesExecute();
    }

    static void BuildResourcesExecute()
    {
        Caching.CleanCache();//使用cach模式加载，第一次指定BundleURL，之后会有缓存，需要清除一下

        string buildPath = BuildUtil.GetBuildDir(target);
        string resPath = Path.Combine(buildPath, "res");

        //if (Directory.Exists(streamPath))
        //{
        //    Directory.Delete(streamPath, true);
        //    if (File.Exists(streamPath + ".meta"))
        //        File.Delete(streamPath + ".meta");
        //}

        AssetDatabase.Refresh();

        ResourcesHandle(resPath);

        BuildUtil.GenVersionFile(buildPath, BuildUtil.VersionEnum.RES);
        BuildUtil.BuildFileIndex(buildPath);
    }

    static void ResourcesHandle(string resPath)
    {
        if (!Directory.Exists(resPath)) Directory.CreateDirectory(resPath);


        //固定打包目标
        string[] respath = new string[]
        {
            Application.dataPath + "/ArtAssets",
            Application.dataPath + "/Prefabs",
            Application.dataPath + "/Shaders"
        };


        List<AssetBundleBuild> abbList = BuildManager.GetBuildAll();
        Debug.Log("收集需要打包的场景");
        //收集需要打包的场景
        foreach (EditorBuildSettingsScene item in EditorBuildSettings.scenes)
        {
            if (!item.enabled || item.path.Contains("GameInitialize.unity")) continue;
            AssetBundleBuild abb = new AssetBundleBuild();
            string bname = item.path.Replace(".unity", FXGame.AppConst.ExtName);
            if (string.IsNullOrEmpty(bname))
                continue;
            abb.assetBundleName = bname.Substring(7);
            abb.assetNames = new string[] { item.path };

            //item.path.ToLower() ==  assets / scenes / gamescenes / scene1.unity
            //abb.assetBundleName.ToLower()  == item.path.ToLower()
            BuildManager.idxMap[item.path.ToLower()] = abb.assetBundleName.ToLower();
            abbList.Add(abb);
        }

        AssetBundleManifest info = BuildPipeline.BuildAssetBundles(resPath, abbList.ToArray(), options, target);
        Debug.Log(info);

        //因为AssetBundleManifest只是一个信息列表，不能通过path找到包，不能查找关联包
        //要建立一个管理器，来处理需求
        //这里面有key，关联ab包， 让我们可以通过路径，查找到所在ab包
        string[] abs = info.GetAllAssetBundles();
        GameAssetsManifest gameAssetsManifest = ScriptableObject.CreateInstance<GameAssetsManifest>();
        gameAssetsManifest.buildPath = resPath;

        //压缩方式
        if ((options & BuildAssetBundleOptions.ChunkBasedCompression) == BuildAssetBundleOptions.ChunkBasedCompression)
            gameAssetsManifest.m_Compression = GameAssetsManifest.AssetBundleCompression.LZ4;
        else if ((options & BuildAssetBundleOptions.UncompressedAssetBundle) == BuildAssetBundleOptions.UncompressedAssetBundle)
            gameAssetsManifest.m_Compression = GameAssetsManifest.AssetBundleCompression.None;
        else
            gameAssetsManifest.m_Compression = GameAssetsManifest.AssetBundleCompression.LZMA;

        gameAssetsManifest.AddAssetBundleManifest(info);
        gameAssetsManifest.AddAssetIdxMap(BuildManager.idxMap);

        //这里的目的是将  GameAssetsManifest 虚拟的管理器，转换成可以在unity显示的二进制文件
        string manifestPath = "Assets/GameAssetsManifest.asset";
        AssetDatabase.CreateAsset(gameAssetsManifest, manifestPath);
        AssetDatabase.Refresh();

        //能打ab包的必须是资源，将转换为二进制文件的GameAssetsManifest，打成ab包
        AssetBundleBuild ab = new AssetBundleBuild();
        ab.assetBundleName = "manifest" + FXGame.AppConst.ExtName;
        ab.assetNames = new string[] { manifestPath };
        BuildPipeline.BuildAssetBundles(resPath, new AssetBundleBuild[] { ab }, options, target);


        Debug.Log(Application.dataPath + "/" + "GameAssetsManifest.asset");//F:/MyGame/AssetsGameAssetsManifest.asset
        Debug.Log(resPath + "/"+ab.assetBundleName + ".manifest");//F:/MyGame/Assets\../AssetBundles\win\resmanifest.png.manifest

        File.Delete(Application.dataPath + "GameAssetsManifest.asset");
        File.Delete(resPath + ab.assetBundleName + ".manifest");
    }

}
