using System;

namespace UEngine.NP.FsmState
{
    [Flags]
    public enum StateType
    {
        NONE = 1 << 0,
        IDLE = 1 << 1,
        Walk = 1 << 2,
        Run = 1 << 3,
        Attack = 1 << 4,
        Patrol = 1 << 5,
        Combo = 1 << 6,
    }
}