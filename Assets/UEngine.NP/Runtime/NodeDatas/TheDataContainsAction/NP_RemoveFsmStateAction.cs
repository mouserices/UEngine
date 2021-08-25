using System;
using Sirenix.OdinInspector;
using UEngine.NP.FsmState;

namespace UEngine.NP
{
    [Title("移除状态",TitleAlignment = TitleAlignments.Centered)]
    public class NP_RemoveFsmStateAction: NP_ClassForStoreAction
    {
        [LabelText("需要切换的状态")]
        public StateType StateType;
        
        public override Action GetActionToBeDone()
        {
            this.Action = () => { BaseUnit.RemoveState(StateType);};
            return this.Action;
        }
    }
}