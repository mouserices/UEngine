using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UEngine.NP;
using UEngine.NP.FsmState;
using UnityEngine;

[Title("播放连击动画", TitleAlignment = TitleAlignments.Centered)]
public class NP_PlayComboAnimAction : NP_BaseAction
{
    [LabelText("需要播放的动画名称")] public string AnimClipName;
    [LabelText("动画速度")] public float AnimClipSpeed = 1;
    [LabelText("第几段连击")]
    public int ComboIndex;
    [LabelText("是否是最后一段连击")]
    public bool IsLastCombo = false;

    public override Action GetActionToBeDone()
    {
        this.Action = () =>
        {
            var gameEntity = Contexts.sharedInstance.game.GetEntityWithUnit(UnitID);
            gameEntity.ReplaceCombo(ComboIndex,0,false);

            gameEntity.ReplaceStateEnter(new AttackStateParam()
                {AnimClipName = AnimClipName, StateType = StateType.Attack, Speed = AnimClipSpeed,OnAttackComplete =
                    () =>
                    {
                        gameEntity.combo.IsComboFinish = true;
                        if (gameEntity.combo.PressedCount <= 0 || IsLastCombo)
                        {
                            Debug.Log($"RemoveCombo frameCount: {Time.frameCount}");
                            gameEntity.RemoveCombo();
                        }
                    }
                });
            
        };
        return this.Action;
    }
}