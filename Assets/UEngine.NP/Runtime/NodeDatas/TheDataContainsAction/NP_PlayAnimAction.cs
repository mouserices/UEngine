using System;
using Sirenix.OdinInspector;
using UEngine.NP.FsmState;
using UEngine.NP.Unit;

namespace UEngine.NP
{
    [Title("播放动画设置", TitleAlignment = TitleAlignments.Centered)]
    public class NP_PlayAnimAction : NP_BaseAction
    {
        // [LabelText("需要切换的状态")]
        // public StateType StateType;

        [LabelText("需要播放的动画名称")] public string AnimClipName;
        [LabelText("动画速度")] public float AnimClipSpeed = 1;

        // [LabelText("状态优先级")]
        // public int Priority = 1;
        //
        // [LabelText("指定时间后自动移除状态")]
        // public int DurationTime = -1;

        public override Action GetActionToBeDone()
        {
            this.Action = () =>
            {
                var gameEntity = Contexts.sharedInstance.game.GetEntityWithUnit(UnitID);

                gameEntity.ReplaceStateEnter(new AttackStateParam()
                    {AnimClipName = AnimClipName, StateType = StateType.Attack, Speed = AnimClipSpeed});
            };
            return this.Action;
        }
    }
}