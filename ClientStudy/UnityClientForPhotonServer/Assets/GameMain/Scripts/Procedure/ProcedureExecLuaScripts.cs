using GameFramework.Fsm;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using UnityGameFramework.Runtime;
using UnityGameFramework.Runtime.Lua;

namespace StarForce
{
    public class ProcedureExecLuaScripts : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            var luaComp = GameEntry.LuaComponent;
            luaComp.Init();
            luaComp.StartLuaVM();
            luaComp.DoMainFile();
        }
    }
}
