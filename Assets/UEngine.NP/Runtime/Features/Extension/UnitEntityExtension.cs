using UEngine.NP.FsmState;

public static class UnitEntityExtension
{
    public static bool CheckState(this GameEntity gameEntity,StateType stateType)
    {
        if (!gameEntity.hasState)
        {
            return false;
        }

        if (gameEntity.state.FsmStateBases.First == null)
        {
            return false;
        }

       return gameEntity.state.FsmStateBases.First.Value.StateType == stateType;
    }
}