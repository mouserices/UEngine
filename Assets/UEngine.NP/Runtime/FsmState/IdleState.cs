namespace UEngine.NP.FsmState
{
    public class IdleState : FsmStateBase
    {
        public override StateType GetConflictStates()
        {
            return StateType.Attack;
        }
    }
}