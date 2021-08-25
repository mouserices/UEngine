namespace UEngine.NP.FsmState
{
    public class RunState: FsmStateBase
    {
        public override StateType GetConflictStates()
        {
            return StateType.NONE;
        }
    }
}