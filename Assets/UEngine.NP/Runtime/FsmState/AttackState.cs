using System.Threading;
using System.Threading.Tasks;

namespace UEngine.NP.FsmState
{
    public class AttackState : FsmStateBase
    {
        private readonly CancellationTokenSource m_CancellationTokenSource = new CancellationTokenSource();
        public override StateType GetConflictStates()
        {
            return StateType.NONE;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            var attackStateParam = StateParam as AttackStateParam;
            
            //play anim
            var gameEntity = entity as GameEntity;
            gameEntity.ReplaceAnimation(attackStateParam.AnimClipName);

            //wait to exist state;
            WaitDurationTime(attackStateParam.DurationTime);
        }

        private async void WaitDurationTime(int durationTime)
        {
            await Task.Delay(durationTime,m_CancellationTokenSource.Token);
            var gameEntity = entity as GameEntity;
            gameEntity.ReplaceStateExit(StateType.Attack);
        }

        public override void OnExist()
        {
            m_CancellationTokenSource.Cancel();
        }
    }
}