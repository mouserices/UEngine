namespace UEngine.NP.FsmState
{
    public class WalkState : FsmStateBase
    {
        public override StateType GetConflictStates()
        {
            return StateType.Attack;
        }
    }
}