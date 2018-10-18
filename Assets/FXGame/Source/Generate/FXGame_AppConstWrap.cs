﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FXGame_AppConstWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FXGame.AppConst), typeof(System.Object));
		L.RegFunction("GetAppDownDir", GetAppDownDir);
		L.RegFunction("New", _CreateFXGame_AppConst);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("DebugMode", get_DebugMode, set_DebugMode);
		L.RegVar("LuaByteMode", get_LuaByteMode, set_LuaByteMode);
		L.RegVar("LuaBundleMode", get_LuaBundleMode, set_LuaBundleMode);
		L.RegVar("VerifyApp", get_VerifyApp, set_VerifyApp);
		L.RegVar("forceLowQuality", get_forceLowQuality, set_forceLowQuality);
		L.RegVar("AppName", get_AppName, null);
		L.RegVar("ExtName", get_ExtName, null);
		L.RegVar("APPRoot", get_APPRoot, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFXGame_AppConst(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				FXGame.AppConst obj = new FXGame.AppConst();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FXGame.AppConst.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAppDownDir(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			string o = FXGame.AppConst.GetAppDownDir();
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DebugMode(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, FXGame.AppConst.DebugMode);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LuaByteMode(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, FXGame.AppConst.LuaByteMode);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LuaBundleMode(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, FXGame.AppConst.LuaBundleMode);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_VerifyApp(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, FXGame.AppConst.VerifyApp);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_forceLowQuality(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, FXGame.AppConst.forceLowQuality);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AppName(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, FXGame.AppConst.AppName);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ExtName(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, FXGame.AppConst.ExtName);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_APPRoot(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, FXGame.AppConst.APPRoot);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_DebugMode(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			FXGame.AppConst.DebugMode = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_LuaByteMode(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			FXGame.AppConst.LuaByteMode = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_LuaBundleMode(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			FXGame.AppConst.LuaBundleMode = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_VerifyApp(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			FXGame.AppConst.VerifyApp = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_forceLowQuality(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			FXGame.AppConst.forceLowQuality = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

