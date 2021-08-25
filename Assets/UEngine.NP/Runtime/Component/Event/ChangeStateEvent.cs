using UEngine.NP.FsmState;

namespace UEngine.NP.Component.Event
{
    public struct ChangeFsmStateEvent
    {
        public bool Processing;
        public StateType StateType;
        public string AnimClipName;
        public int Priority;
        public int DurationTime;
    }
}