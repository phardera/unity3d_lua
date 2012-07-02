using UnityEngine;
using System.Collections;

using System;
using System.Runtime.InteropServices;

public class LuaScriptTest : MonoBehaviour {
	
	[DllImport("lua514vc")] private static extern IntPtr luaL_newstate();
	[DllImport("lua514vc")] private static extern void luaL_openlibs(IntPtr lua_State);
	[DllImport("lua514vc")] private static extern void lua_close(IntPtr lua_State);
	[DllImport("lua514vc")] private static extern void lua_pushcclosure(IntPtr lua_State, [MarshalAs(UnmanagedType.FunctionPtr)] LuaFunction func, int n);
	[DllImport("lua514vc")] private static extern void lua_setfield(IntPtr lua_State, int idx, string s);
	[DllImport("lua514vc")] private static extern int lua_pcall(IntPtr lua_State, int nargs, int nresults, int errfunc);
	[DllImport("lua514vc")] private static extern int luaL_loadfile(IntPtr lua_State, string s);
	
    public enum LuaIndexes
    {
        LUA_REGISTRYINDEX = -10000,
        LUA_ENVIRONINDEX = -10001,
        LUA_GLOBALSINDEX = -10002
    }
	
	protected IntPtr m_luaState =IntPtr.Zero;
	
	public delegate int LuaFunction(IntPtr pLuaState);
	protected static void lua_register(IntPtr pLuaState, string strFuncName, LuaFunction pFunc)
	{
    	lua_pushcclosure(pLuaState, pFunc, 0);
    	lua_setfield(pLuaState, (int)LuaIndexes.LUA_GLOBALSINDEX, strFuncName);
	}
	
	protected static int luaL_dofile(IntPtr lua_State, string s)
	{
    	if (luaL_loadfile(lua_State, s) != 0)
        	return 1;
 
    	return lua_pcall(lua_State, 0, -1, 0);
	}
	
	// Use this for initialization
	void Start () {
		
		Debug.Log("init lua...");
		m_luaState =luaL_newstate();
		luaL_openlibs(m_luaState);
		
		Debug.Log("register func. ...");
		lua_register(m_luaState, "TestDisplay", TestDisplay);
		
		Debug.Log("do lua script...");
		luaL_dofile( m_luaState, "test.txt" );
	
	}
	
	void OnApplicationQuit()
	{
		Debug.Log("close lua...");
		lua_close(m_luaState);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	int TestDisplay(IntPtr pLuaState)
	{
    	Debug.Log("This line was plotted by TestDisplay()");
    	return 0;
	}
}
