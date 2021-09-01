using UEngine.NP.FsmState;

namespace UEngine.NP.Features.FsmState
{
    public class IdleStateParam : IStateParam
    {
        public string AnimClipName;
        public StateType GetStateType()
        {
            return StateType.IDLE;
        }
    }
}