﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UnityEngine_UI_CanvasScaler_ScreenMatchModeWrap
{
	public static void Register(LuaState L)
	{
		L.BeginEnum(typeof(UnityEngine.UI.CanvasScaler.ScreenMatchMode));
		L.RegVar("MatchWidthOrHeight", get_MatchWidthOrHeight, null);
		L.RegVar("Expand", get_Expand, null);
		L.RegVar("Shrink", get_Shrink, null);
		L.RegFunction("IntToEnum", IntToEnum);
		L.EndEnum();
		TypeTraits<UnityEngine.UI.CanvasScaler.ScreenMatchMode>.Check = CheckType;
		StackTraits<UnityEngine.UI.CanvasScaler.ScreenMatchMode>.Push = Push;
	}

	static void Push(IntPtr L, UnityEngine.UI.CanvasScaler.ScreenMatchMode arg)
	{
		ToLua.Push(L, arg);
	}

	static bool CheckType(IntPtr L, int pos)
	{
		return TypeChecker.CheckEnumType(typeof(UnityEngine.UI.CanvasScaler.ScreenMatchMode), L, pos);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MatchWidthOrHeight(IntPtr L)
	{
		ToLua.Push(L, UnityEngine.UI.CanvasScaler.ScreenMatchMode.MatchWidthOrHeight);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Expand(IntPtr L)
	{
		ToLua.Push(L, UnityEngine.UI.CanvasScaler.ScreenMatchMode.Expand);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Shrink(IntPtr L)
	{
		ToLua.Push(L, UnityEngine.UI.CanvasScaler.ScreenMatchMode.Shrink);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		UnityEngine.UI.CanvasScaler.ScreenMatchMode o = (UnityEngine.UI.CanvasScaler.ScreenMatchMode)arg0;
		ToLua.Push(L, o);
		return 1;
	}
}

