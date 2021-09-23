using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UEngine.NP;
using UnityEngine;

[Title("清除连击次数", TitleAlignment = TitleAlignments.Centered)]
public class NP_ClearComboAction : NP_ClassForStoreAction
{
    [LabelText("清除延迟时间")]
    public float DelayClearTime;
    public override Action GetActionToBeDone()
    {
        this.Action = () =>
        {
            // var entity = Contexts.sharedInstance.game.GetEntityWithUnit(this.Skill.UnitID);
            // if (entity.hasInputKeyRefenercedTimer)
            // {
            //     if (entity.inputKeyRefenercedTimer.Timer != null)
            //     {
            //         entity.inputKeyRefenercedTimer.Timer.RemoveTimer();
            //         entity.inputKeyRefenercedTimer.Timer.isDestroyed = true;
            //         entity.inputKeyRefenercedTimer.Timer = null;
            //     }
            // }
            //
            // var timerEntity = Contexts.sharedInstance.game.CreateEntity();
            // timerEntity.ReplaceTimer(DelayClearTime);
            // timerEntity.ReplaceTimerCompleteAction(() =>
            // {
            //     if (entity.hasCombo)
            //     {
            //         entity.ReplaceCombo(0);
            //     }
            // });
        };
        return this.Action;
    }
}