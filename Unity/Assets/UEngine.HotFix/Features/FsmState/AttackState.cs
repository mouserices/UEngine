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

#if CLIENT
           //play anim
            var gameEntity = entity as UnitEntity;
            gameEntity.ReplaceAnimation(attackStateParam.AnimClipName,attackStateParam.Speed, () =>
            {
                gameEntity.ReplaceStateExit(StateType.Attack);
                attackStateParam.OnAttackComplete?.Invoke();
            }); 
#endif
        }

        private async void WaitDurationTime(int durationTime)
        {
            await Task.Delay(durationTime,m_CancellationTokenSource.Token);
        }

        public override void OnExist()
        {
            m_CancellationTokenSource.Cancel();
        }
    }
}