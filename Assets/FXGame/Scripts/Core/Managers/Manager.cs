using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FXGame.Managers
{
    /// <summary>
    /// 管理器单例基类
    /// </summary>
    public class Manager<T> : MonoBehaviour where T : Component
    {
        private static Dictionary<System.Type, Component> m_managers = new Dictionary<System.Type, Component>();
        //封装方法
        public static T Instance
        {
            get
            {
                Type type = typeof(T);
                if (!m_managers.ContainsKey(type))
                {
                    Manager<T> manager = GameObject.FindObjectOfType(type) as Manager<T>;
                    if (manager == null)
                    {
                        GameObject go = GameObject.Find(AppConst.AppName);
                        if (go == null)
                        {
                            go = new GameObject(AppConst.AppName);
                            DontDestroyOnLoad(go);
                        }
                        manager = go.AddComponent(type) as Manager<T>;
                    }
                    if (!m_managers.ContainsKey(type))
                    {
                        m_managers.Add(type, manager);
                    }
                }
                return (T)m_managers[type];
            }
        }


        public virtual void Awake()
        {
            Component m = Instance as Component;
        }
    }
}

