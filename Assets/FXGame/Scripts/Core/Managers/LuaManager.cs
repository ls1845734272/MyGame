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

			this.OpenLibs();//这里是添加调试luaide
			lua.LuaSetTop(0);

            float t = Time.realtimeSinceStartup;
            LuaBinder.Bind(lua);
            Debug.Log("LuaBinder Wrap注册：" + (Time.realtimeSinceStartup - t) + "s");

			LuaCoroutine.Register(lua, this);//携程
            
			DelegateFactory.Init();
            lua.Start();

            string fullPath = Application.dataPath + "/FXGame/Lua";
            lua.AddSearchPath(fullPath);
            fullPath = Application.dataPath + "/FXGame/ToLua/Lua";
            lua.AddSearchPath(fullPath);

            StartMain();
            StartLooper();//这里是处理update的方法，不调用这里计时器执行一次
        }

		/// <summary>
		/// 初始化加载第三方库
		/// </summary>
		void OpenLibs() {
			// lua.OpenLibs(LuaDLL.luaopen_pb);
			//lua.OpenLibs(LuaDLL.luaopen_sproto_core);
			//lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
			// lua.OpenLibs(LuaDLL.luaopen_lpeg);
			lua.OpenLibs(LuaDLL.luaopen_bit);
			lua.OpenLibs(LuaDLL.luaopen_socket_core);
			lua.LuaCheckStack(200);

			this.OpenCJson();

			if (AppConst.DebugMode) {
				if (LuaConst.openLuaSocket)
				{
					OpenLuaSocket();
				}
			}
		}

		//cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
		protected void OpenCJson() {
			lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
			lua.OpenLibs(LuaDLL.luaopen_cjson);
			lua.LuaSetField(-2, "cjson");

			lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
			lua.LuaSetField(-2, "cjson.safe");


		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int LuaOpen_Socket_Core(IntPtr L)
		{
			return LuaDLL.luaopen_socket_core(L);
		}


		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int LuaOpen_Mime_Core(IntPtr L)
		{
			return LuaDLL.luaopen_mime_core(L);
		}

		protected void OpenLuaSocket()
		{
			LuaConst.openLuaSocket = true;

			lua.BeginPreLoad();
			lua.RegFunction("socket.core", LuaOpen_Socket_Core);
			lua.RegFunction("mime.core", LuaOpen_Mime_Core);
			lua.EndPreLoad();
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

		public void Close()
		{
			loop.Destroy();
			loop = null;

			lua.Dispose();
			lua = null;
		}

        public void OnDestroy()
        {
            Close();
        }

    }

}
