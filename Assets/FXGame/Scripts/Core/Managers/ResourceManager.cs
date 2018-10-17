using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FXGame.Managers
{
    public class ResourceManager : Manager<ResourceManager>
    {

        private Dictionary<string, System.Object> objDic = new Dictionary<string, System.Object>();


        public System.Object LoadExecute(string res, System.Type type,bool isAsync = false)
        {
            try
            {
                if (!CheckPath(res)) return null;
                //缓存
                if (objDic.ContainsKey(res))
                {
                    if (objDic[res].Equals(null) || objDic[res] == null)
                        objDic.Remove(res);
                    else
                        return objDic[res];
                }
                System.Object obj = null;
                if (AppConst.DebugMode)
                {
#if UNITY_EDITOR
                    obj = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/" + res, type);
#endif
                }
                else
                {
                    //string resName = Path.GetFileName(res);
                    //string abpath = AssetsBundleManager.Instance.GetAssetBundleNameAtPath(res);
                    //if (string.IsNullOrEmpty(abpath))
                    //{
                    //    //Debug.Log(res + "的ab包文件不存在");
                    //    return obj;
                    //}
                    //AssetBundle ab = AssetsBundleManager.Instance.GetAssetBundle(abpath);

                    //if (!resName.EndsWith(".unity") && ab)
                    //{
                    //    if (isAsync)
                    //    {
                    //        obj = ab.LoadAssetAsync(resName, type);
                    //    }
                    //    else
                    //    {
                    //        UnityEngine.Profiling.Profiler.BeginSample("ResourceManager -> LoadExecute -> ab.LoadAsset");
                    //        obj = ab.LoadAsset(resName, type);
                    //        UnityEngine.Profiling.Profiler.EndSample();

                    //        objDic.Add(res, obj);
                    //    }
                    //}
                }
                return obj;

            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            return null;
        }


        public Object Load(string res, System.Type type)
        {
            return (Object)LoadExecute(res, type);
        }


        /// <summary>
        /// 加载对象并实例化到场景
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameObject LoadInstantiate<T>(string path)
        {
            GameObject go = (GameObject)LoadInstantiate(path, typeof(T));
            return go;
        }


        public GameObject LoadInstantiate(string path,System.Type type)
        {
            GameObject go = (GameObject)Load(path, type);
            go = go ? Instantiate(go) : null;
            return go;
        }


        /// <summary>
        /// 检查路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CheckPath(string path)
        {
            if (!Path.HasExtension(path))
            { Debug.Log("<color=#ff0000>资源路径无扩展名: </color>" + path);
                return false;
            }
            return true;
        }
    }
}

