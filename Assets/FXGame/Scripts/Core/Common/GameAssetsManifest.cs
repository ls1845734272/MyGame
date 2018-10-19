using UnityEngine;
using System.Collections.Generic;
public class GameAssetsManifest : ScriptableObject, ISerializationCallbackReceiver
{
    [System.Serializable]
    struct AssetBundleInfo
    {
        public uint m_Crc;
        public string m_AssetBundleHash;
        public int[] m_AssetBundleDependencies;
    }

    public enum AssetBundleCompression
    {
        None, LZ4, LZMA,
    }

    [SerializeField]
    private List<string> m_Keys = new List<string>();
    [SerializeField]
    private List<int> m_Values = new List<int>();
    [SerializeField]
    private List<string> m_AssetBundleNames = new List<string>();//包名字
    private Dictionary<string, int> m_AssetNamesIdxMap = new Dictionary<string, int>();
    [SerializeField]
    private List<AssetBundleInfo> m_AssetBundleInfos = new List<AssetBundleInfo>();
    private Dictionary<string, int> m_AssetIdxMap = new Dictionary<string, int>();//根据路径，获得ab包的索引；； 在m_AssetBundleNames里面，通过索引获得ab包名字 
    public AssetBundleCompression m_Compression = AssetBundleCompression.None;
    //#if UNITY_EDITOR
    //[System.NonSerialized]
    public string buildPath = string.Empty;
    //#endif
    public void OnBeforeSerialize()
    {
        m_Keys.Clear();
        m_Values.Clear();
        foreach (var item in m_AssetIdxMap)
        {
            m_Keys.Add(item.Key);
            m_Values.Add(item.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        m_AssetIdxMap.Clear();
        for (int i = 0; i != Mathf.Min(m_Keys.Count, m_Values.Count); i++)
            m_AssetIdxMap.Add(m_Keys[i], m_Values[i]);


        m_AssetNamesIdxMap.Clear();
        int count = m_AssetBundleNames.Count;
        for (int i = 0; i < count; i++)
            m_AssetNamesIdxMap.Add(m_AssetBundleNames[i], i);
    }


    int GetAssetBundleIdxAlName(string assetBundleName)
    {
        return m_AssetNamesIdxMap.ContainsKey(assetBundleName) ? m_AssetNamesIdxMap[assetBundleName] : m_AssetBundleNames.IndexOf(assetBundleName);
    }

    int[] GetAssetBundleIdxAlName(string[] assetBundleNames)
    {
        if (assetBundleNames != null && assetBundleNames.Length > 0)
        {
            int[] depidxs = new int[assetBundleNames.Length];
            for (int i = 0; i < assetBundleNames.Length; i++)
                depidxs[i] = GetAssetBundleIdxAlName(assetBundleNames[i]);
            return depidxs;
        }
        return null;
    }

    string GetAssetNameAtIdx(int idx)
    {
        if (idx < 0 || idx > m_AssetBundleNames.Count - 1)
            return null;
        return m_AssetBundleNames[idx];
    }

    string[] GetAssetNameAtIdx(int[] idxs)
    {
        if (idxs != null && idxs.Length > 0)
        {
            string[] result = new string[idxs.Length];
            for (int i = 0; i < idxs.Length; i++)
                result[i] = GetAssetNameAtIdx(idxs[i]);
            return result;
        }
        return null;
    }

    AssetBundleInfo? GetAssetBundleInfo(string assetBundleName)
    {
        int idx = GetAssetBundleIdxAlName(assetBundleName);
        if (idx < 0 || idx > m_AssetBundleNames.Count - 1)
            return null;
        return m_AssetBundleInfos[idx];
    }

    public void AddAssetBundleManifest(AssetBundleManifest abm)
    {
        AddAssetBundleManifestInternal(abm);
    }

    public void AddAssetBundleManifest(AssetBundleManifest[] abms)
    {
        for (int i = 0; i < abms.Length; i++)
            AddAssetBundleManifestInternal(abms[i]);
    }

    void AddAssetBundleManifestInternal(AssetBundleManifest abm)
    {
        string[] abmAllAssetBundle = abm.GetAllAssetBundles();
        for (int i = 0; i < abmAllAssetBundle.Length; i++)
        {
            if (!m_AssetBundleNames.Contains(abmAllAssetBundle[i]))
                m_AssetBundleNames.Add(abmAllAssetBundle[i]);
        }

#if UNITY_EDITOR
        bool checkCrc = string.IsNullOrEmpty(buildPath);
#endif
        string assetBundleName;
        for (int i = 0; i < abmAllAssetBundle.Length; i++)
        {
            assetBundleName = abmAllAssetBundle[i];
            AssetBundleInfo info = new AssetBundleInfo();
            info.m_AssetBundleHash = abm.GetAssetBundleHash(assetBundleName).ToString();
            info.m_AssetBundleDependencies = GetAssetBundleIdxAlName(abm.GetAllDependencies(assetBundleName));

#if UNITY_EDITOR
            uint crc;
            string fullPath = System.IO.Path.Combine(buildPath, assetBundleName);
            if (!UnityEditor.BuildPipeline.GetCRCForAssetBundle(fullPath, out crc))
                Debug.LogWarning("GetCRCForAssetBundle false name:" + assetBundleName);
            else
                info.m_Crc = crc;
#endif

            m_AssetBundleInfos.Insert(m_AssetBundleNames.IndexOf(assetBundleName), info);
        }
    }

    public void AddAssetIdxMap(Dictionary<string, string> idxMap)
    {
        string key, value;
        foreach (var item in idxMap)
        {
            value = item.Value.ToLower().Replace("\\", "/");
            key = item.Key.ToLower().Replace("\\", "/");
            int idx = m_AssetBundleNames.IndexOf(value);
            if (idx != -1)
            {
                if (m_AssetIdxMap.ContainsKey(key))
                    m_AssetIdxMap.Remove(key);
                m_AssetIdxMap.Add(key, idx);
            }
        }
    }
    //通过路径来查到ab索引
    public string GetAssetBundleNameAtPath(string assetPath)
    {
        assetPath = "assets/" + assetPath.ToLower().Replace("\\", "/");
        int idx = -1;
        if (!m_AssetIdxMap.TryGetValue(assetPath, out idx))
            return string.Empty;
        if (idx < 0 || idx > m_AssetBundleNames.Count - 1)
            return string.Empty;
        return m_AssetBundleNames[idx];
    }

    string[] temp = new string[0];
    public string[] GetAllDependencies(string assetBundleName)
    {
        int idx = GetAssetBundleIdxAlName(assetBundleName);
        if (idx < 0 || idx > m_AssetBundleNames.Count - 1)
            return temp;
        if (m_AssetBundleInfos[idx].m_AssetBundleDependencies != null && m_AssetBundleInfos[idx].m_AssetBundleDependencies.Length > 0)
            return GetAssetNameAtIdx(m_AssetBundleInfos[idx].m_AssetBundleDependencies);
        return temp;
    }

    public string GetAssetBundleHash(string assetBundleName)
    {
        AssetBundleInfo? info = GetAssetBundleInfo(assetBundleName);
        if (info != null && info.HasValue)
            return info.Value.m_AssetBundleHash;
        return "";
    }

    public uint GetAssetBundleCrc(string assetBundleName)
    {
        AssetBundleInfo? info = GetAssetBundleInfo(assetBundleName);
        if (info != null && info.HasValue)
            return info.Value.m_Crc;
        return 0;
    }
}
