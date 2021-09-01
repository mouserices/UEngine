using UEngine.NP.FsmState;


public class AttackStateParam : IStateParam
{
    public string AnimClipName;
    public int DurationTime;
    public StateType GetStateType()
    {
        return StateType.Attack;
    }
}