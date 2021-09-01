using UEngine.NP.FsmState;

public class WalkStateParam : IStateParam
{
    public string AnimClipName;

    public StateType GetStateType()
    {
        return StateType.Walk;
    }
}