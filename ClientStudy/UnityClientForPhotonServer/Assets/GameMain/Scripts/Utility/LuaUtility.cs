
namespace StarForce
{
    public static class LuaUtility
    {
        /// <summary>
        /// 执行某模块内的Lua方法
        /// <param name="module">模块名</param>
        /// <param name="func">方法名</param>
        /// <param name="args">有GCAlloc的参数</param>
        /// <returns>有返回值</returns>
        /// </summary>
        //public static object[] CallMethod(string module, string func, params object[] args)
        //{
        //    if (GameEntry.LuaComponent == null)
        //        return null;

        //    return GameEntry.LuaComponent.CallFunction(module + "." + func, args);

        //}

        /// <summary>
        /// 执行某模块内的Lua方法
        /// <param name="module">模块名</param>
        /// <param name="func">方法名</param>
        /// <param name="arg">无GCAlloc的泛型参数</param>
        /// </summary>
        public static void CallMethod<T>(string module, string func, T arg)
        {
            if (GameEntry.LuaComponent == null)
                return;

            GameEntry.LuaComponent.CallFunction(module + "." + func, arg);

        }

        /// <summary>
        /// 执行某模块内的Lua方法
        /// <param name="module">模块名</param>
        /// <param name="func">方法名</param>
        /// <param name="arg0">无GCAlloc的泛型参数</param>
        /// <param name="arg1">无GCAlloc的泛型参数</param>
        /// </summary>
        public static void CallMethod<T0, T1>(string module, string func, T0 arg0, T1 arg1)
        {
            if (GameEntry.LuaComponent == null)
                return;

            GameEntry.LuaComponent.CallFunction(module + "." + func, arg0, arg1);
        }

        /// <summary>
        /// 执行某模块内的Lua方法
        /// <param name="module">模块名</param>
        /// <param name="func">方法名</param>
        /// <param name="arg0">无GCAlloc的泛型参数</param>
        /// <param name="arg1">无GCAlloc的泛型参数</param>
        /// <param name="arg2">无GCAlloc的泛型参数</param>
        /// </summary>
        public static void CallMethod<T0, T1, T2>(string module, string func, T0 arg0, T1 arg1, T2 arg2)
        {
            if (GameEntry.LuaComponent == null)
                return;

            GameEntry.LuaComponent.CallFunction(module + "." + func, arg0, arg1, arg2);
        }

        /// <summary>
        /// 执行某模块内的Lua方法
        /// <param name="module">模块名</param>
        /// <param name="func">方法名</param>
        /// <param name="arg0">无GCAlloc的泛型参数</param>
        /// <param name="arg1">无GCAlloc的泛型参数</param>
        /// <param name="arg2">无GCAlloc的泛型参数</param>
        /// <param name="arg3">无GCAlloc的泛型参数</param>
        /// </summary>
        public static void CallMethod<T0, T1, T2, T3>(string module, string func, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            if (GameEntry.LuaComponent == null)
                return;

            GameEntry.LuaComponent.CallFunction(module + "." + func, arg0, arg1, arg2, arg3);
        }
    }

}
