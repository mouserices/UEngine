using NPBehave;
using UEngine.NP.Features.FsmState;
using Task = System.Threading.Tasks.Task;

namespace UEngine.NP.FsmState
{
    public class IdleState : FsmStateBase
    {
        public override StateType GetConflictStates()
        {
            return StateType.Attack | StateType.Combo;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            var idleStateParam = StateParam as IdleStateParam;
#if CLIENT
            //play anim
            var gameEntity = entity as UnitEntity;
            gameEntity.ReplaceAnimation(idleStateParam.AnimClipName,1,null);
#endif
            
        }
    }
}