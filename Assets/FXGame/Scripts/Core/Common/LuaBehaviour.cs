using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FXGame
{
    [Serializable]
    public class ObjMap
    {
        public string name;
        public int idx;
        public string compName;
        public GameObject go;
    }

    public class LuaBehaviour : FXBehaviour
    {
        public string className;
        // Is ready or not.
        private bool m_bReady = false;

        private bool hasAwake = false;

        public List<ObjMap> objs = new List<ObjMap>();

        // The lua behavior.
        private LLuaBehaviourInterface m_cBehavior = new LLuaBehaviourInterface();

        [Tooltip("开启此选项将会 把对象列表 注入到 luaName 对像身上 无需代码Get引用 \n 注意属性名皆以 inje_ + 对象名 开头 如 inje_button1 = 1")]
        public bool injectToLua = true;
        private Dictionary<string, LuaFunction> buttons = new Dictionary<string, LuaFunction>();

        // The awake method.
        void Awake()
        {
            if (!enabled) return;

            if (className == string.Empty)
            {
                //Debug.LogWarning("lua class name Invalid:" + gameObject.name);
                return;
            }
            // Directly creat a lua class instance to associate with this monobehavior.
            if (!CreateClassInstance(className) || !m_bReady)
            {
                return;
            }

            if (hasAwake == false)
            {
                if (injectToLua) InjectObjectToLua();
                m_cBehavior.Awake();
            }
        }

        void OnEnable()
        {
            if (m_bReady)
            {
                m_cBehavior.OnEnable();
            }
        }

        void Start()
        {
            if (m_bReady)
            {
                m_cBehavior.Start();
            }
        }

        void OnDestroy()
        {
            if (m_bReady)
            {
                m_cBehavior.OnDestroy();
                ClearInject();
            }
        }

        void ClearInject()
        {
            if (string.IsNullOrEmpty(className)) return;
            LuaTable tab = GetInstance();
            if (null == tab) return;
            int len = objs.Count;
            for (int i = 0; i < len; i++)
            {
                if (null != tab["inje_" + objs[i].name])
                    tab["inje_" + objs[i].name] = null;
            }
        }

        /// <summary>
        /// 方便层级修改后代码也要相应变动
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetGameObject(string name)
        {
            for (int i = 0; i < objs.Count; i++)
                if (objs[i].name.Equals(name)) return objs[i].go;
            return null;
        }

        /// <summary>
        /// 添加单击事件
        /// </summary>
        public void AddClick(GameObject go, LuaFunction luafunc)
        {
            if (go == null || luafunc == null) return;
            buttons.Add(go.name, luafunc);
            go.GetComponent<Button>().onClick.AddListener(
                delegate ()
                {
                    luafunc.Call(go);
                }
            );
        }

        /// <summary>
        /// 删除单击事件
        /// </summary>
        /// <param name="go"></param>
        public void RemoveClick(GameObject go)
        {
            if (go == null) return;
            LuaFunction luafunc = null;
            if (buttons.TryGetValue(go.name, out luafunc))
            {
                luafunc.Dispose();
                luafunc = null;
                buttons.Remove(go.name);
            }
        }

        /// <summary>
        /// 清除单击事件
        /// </summary>
        public void ClearClick()
        {
            foreach (var de in buttons)
            {
                if (de.Value != null)
                {
                    de.Value.Dispose();
                }
            }
            buttons.Clear();
        }

        private void InjectObjectToLua()
        {
            if (string.IsNullOrEmpty(className)) return;
            LuaTable tab = GetInstance();
            if (null == tab) return;

            int len = objs.Count;
            for (int i = 0; i < len; i++)
            {
                if (null != tab["inje_" + objs[i].name] && null != objs[i].go)
                {
                    if (string.IsNullOrEmpty(objs[i].compName) || objs[i].compName == "GameObject")
                        tab["inje_" + objs[i].name] = objs[i].go;
                    else
                        tab["inje_" + objs[i].name] = objs[i].go.GetComponent(objs[i].compName);
                }
            }
        }

        /**
         * Get the lua class instance (Actually a lua table).
         * 
         * @param void.
         * @return LuaTable - The class instance table..
         */
        public LuaInterface.LuaTable GetInstance()
        {
            if (m_bReady == false)
            {
                CreateClassInstance(className);
            }
            return m_cBehavior.GetChunk();
        }


        /**
         * Create a lua class instance for monobehavior instead of do a file.
         * 
         * @param string strFile - The lua class name.
         * @return bool - true if success, otherwise false.
         */
        private bool CreateClassInstance(string strClassName)
        {
            if (!m_cBehavior.CreateClassInstance(strClassName))
            {
                return false;
            }

            // Init variables.
            m_cBehavior.SetData("this", this);
            m_cBehavior.SetData("transform", transform);
            m_cBehavior.SetData("gameObject", gameObject);

            LuaTable tab = m_cBehavior.GetChunk();
            if (null == tab) return false;

            int len = objs.Count;
            for (int i = 0; i < len; i++)
            {
                if (null != tab["inje_" + objs[i].name] && null != objs[i].go)
                {
                    if (string.IsNullOrEmpty(objs[i].compName) || objs[i].compName == "GameObject")
                        tab["inje_" + objs[i].name] = objs[i].go;
                    else
                        tab["inje_" + objs[i].name] = objs[i].go.GetComponent(objs[i].compName);
                }
            }
            m_cBehavior.Awake();

            m_bReady = true;
            hasAwake = true;
            return true;
        }

    }
}

