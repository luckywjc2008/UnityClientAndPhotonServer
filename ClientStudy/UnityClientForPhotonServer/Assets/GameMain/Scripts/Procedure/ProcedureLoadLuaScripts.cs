using GameFramework;
using GameFramework.Procedure;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityGameFramework.Runtime.Lua;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace StarForce
{
    public class ProcedureLoadLuaScripts : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        private class LuaScriptInfo
        {
            public string AssetPathPrefix;
            public string FileName;
        }

        private static readonly LuaScriptInfo[] PreloadLuaScriptsInfos = new LuaScriptInfo[]
        {
            new LuaScriptInfo { AssetPathPrefix = "Assets/GameMain/LuaScripts", FileName = "Main.lua" },
        };

        private HashSet<string> m_LoadFlags = new HashSet<string>();

        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            var luaComp = GameEntry.LuaComponent;

            if (Application.isEditor && GameEntry.Base.EditorResourceMode)
            {
                return;
            }

            for (int i = 0; i < PreloadLuaScriptsInfos.Length; ++i)
            {
                var info = PreloadLuaScriptsInfos[i];
                m_LoadFlags.Add(info.FileName);
                luaComp.LoadFile(Utility.Path.GetCombinePath(info.AssetPathPrefix , info.FileName), info.FileName, OnLoadLuaScriptSuccess, OnLoadLuaScriptFailure);
            }
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_LoadFlags.Count <= 0)
            {
                ChangeState<ProcedureExecLuaScripts>(procedureOwner);
            }
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            m_LoadFlags.Clear();
            base.OnLeave(procedureOwner, isShutdown);
        }

        private void OnLoadLuaScriptSuccess(string fileName)
        {
            Log.Info("Load lua script '{0}' success.", fileName);
            m_LoadFlags.Remove(fileName);
        }

        private void OnLoadLuaScriptFailure(string fileName, LoadResourceStatus status, string errorMessage)
        {
            Log.Warning("Load lua script '{0}' failure. Status is '{1}'. Error message is '{2}'.", fileName, status, errorMessage);
        }
    }
}
