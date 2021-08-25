using System;
using Sirenix.OdinInspector;
using UEngine.NP.FsmState;

namespace UEngine.NP
{
    [Title("切换状态",TitleAlignment = TitleAlignments.Centered)]
    public class Np_ChangeFsmStateAction : NP_ClassForStoreAction
    {

        [LabelText("需要切换的状态")]
        public StateType StateType;
        
        [LabelText("需要播放的动画名称")]
        public string AnimClipName;

        [LabelText("状态优先级")]
        public int Priority = 1;
        
        [LabelText("指定时间后自动移除状态")]
        public int DurationTime = -1;

        public override Action GetActionToBeDone()
        {
            this.Action = () => { BaseUnit.ChangeState(StateType,AnimClipName,Priority,DurationTime);};
            return this.Action;
        }
    }
}