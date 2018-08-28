using UnityEngine;
using System.Collections;
using LuaInterface;
using System;
using System.IO;

namespace FXGame.Managers
{
    public class LuaManager : Manager<LuaManager>
    {

        private LuaState lua;
        private LuaLooper loop = null;
        // Use this for initialization
        public void TestStart()
        {
            //GameObject.DontDestroyOnLoad(gameObject);
            new LuaResLoader();
            lua = new LuaState();

            float t = Time.realtimeSinceStartup;
            LuaBinder.Bind(lua);
            Debug.Log("LuaBinder Wrap注册：" + (Time.realtimeSinceStartup - t) + "s");
            DelegateFactory.Init();
            lua.Start();

            string fullPath = Application.dataPath + "/FXGame/Lua";
            lua.AddSearchPath(fullPath);
            fullPath = Application.dataPath + "/FXGame/ToLua/Lua";
            lua.AddSearchPath(fullPath);

            StartMain();
            StartLooper();
        }



        void StartMain()
        {
            lua.DoFile("Game/Main.lua");
            LuaFunction main = lua.GetFunction("main");
            main.Call();
            main.Dispose();
            main = null;


            //main.BeginPCall();
            //main.PCall();
            //main.EndPCall();

            //main.Dispose();
            //main = null;
        }
        void StartLooper()
        {
            loop = gameObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
        }

        public LuaTable GetLuaTable(string name)
        {
            return lua.GetTable(name);
        }


        public void OnDestroy()
        {
            Close();
        }
        public void Close()
        {
            loop.Destroy();
            loop = null;

            lua.Dispose();
            lua = null;
        }
    }

}
