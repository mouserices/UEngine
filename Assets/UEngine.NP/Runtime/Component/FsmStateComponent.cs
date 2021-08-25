using System.Collections.Generic;
using UEngine.NP.FsmState;

namespace UEngine.NP.Component
{
    public struct FsmStateComponent
    {
        public Dictionary<StateType, FsmStateBase> FsmStateBases;
        public LinkedList<FsmStateBase> RuntimeFsmStateBases;
        public FsmStateBase CurState;
    }
}