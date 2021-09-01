using NPBehave;
using UEngine.NP.Features.FsmState;
using Task = System.Threading.Tasks.Task;

namespace UEngine.NP.FsmState
{
    public class IdleState : FsmStateBase
    {
        public override StateType GetConflictStates()
        {
            return StateType.Attack;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            var idleStateParam = StateParam as IdleStateParam;
            
            //play anim
            var gameEntity = entity as GameEntity;
            gameEntity.ReplaceAnimation(idleStateParam.AnimClipName);
        }
    }
}