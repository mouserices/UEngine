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
            WaitDurationTime(this.DurationTime);
        }

        private async void WaitDurationTime(int durationTime)
        {
            await Task.Delay(durationTime,m_CancellationTokenSource.Token);
            BaseUnit.RemoveState(FsmState.StateType.Attack);
        }

        public override void OnExist()
        {
            m_CancellationTokenSource.Cancel();
        }
    }
}