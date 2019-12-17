using UnityEngine;
using System.Collections;
using LuaInterface;
using System.Collections.Generic;

namespace UnityGameFramework.Runtime.Lua
{
    public static class ToLuaExtensions
    {
        public static void BetterCall<T>(this LuaFunction func, T arg)
        {
            func.BeginPCall();
            func.Push(arg);
            func.PCall();
            func.EndPCall();
        }

        public static void BetterCall<T0, T1>(this LuaFunction func, T0 arg0, T1 arg1)
        {
            func.BeginPCall();
            func.Push(arg0);
            func.Push(arg1);
            func.PCall();
            func.EndPCall();
        }

        public static void BetterCall<T0, T1, T2>(this LuaFunction func, T0 arg0, T1 arg1, T2 arg2)
        {
            func.BeginPCall();
            func.Push(arg0);
            func.Push(arg1);
            func.Push(arg2);
            func.PCall();
            func.EndPCall();
        }

        public static void BetterCall<T0, T1, T2, T3>(this LuaFunction func, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            func.BeginPCall();
            func.Push(arg0);
            func.Push(arg1);
            func.Push(arg2);
            func.Push(arg3);
            func.PCall();
            func.EndPCall();
        }

    }
}

