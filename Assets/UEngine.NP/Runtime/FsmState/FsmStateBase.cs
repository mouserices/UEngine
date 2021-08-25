using UEngine.NP.Unit;

namespace UEngine.NP.FsmState
{
    public abstract class FsmStateBase
    {
        public BaseUnit BaseUnit;
        public StateType StateType;
        public int Priority;
        public int DurationTime;
        public string AnimClipName;

        public virtual void OnEnter()
        {
            if (!string.IsNullOrEmpty(AnimClipName))
            {
                BaseUnit.PlayAnim(AnimClipName);
            }
        }

        public virtual void OnRemove()
        {
        }

        public virtual void OnExist()
        {
        }

        public virtual bool TryEnter(StateType stateType)
        {
            var conflictStates = GetConflictStates();
            if ((conflictStates & stateType) == stateType)
            {
                return false;
            }
            return true;
        }

        public abstract StateType GetConflictStates();
    }
}