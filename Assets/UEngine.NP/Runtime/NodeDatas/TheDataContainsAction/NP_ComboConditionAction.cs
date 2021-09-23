using System;
using Sirenix.OdinInspector;
using UEngine.NP;
using UnityEngine;

public class NP_ComboConditionAction : NP_ClassForStoreAction
{
    [LabelText("监听的哪个Key")] public KeyCode keyCode;
    [LabelText("监听的连击次数")] public int Num;

    public override Func<bool> GetFunc1ToBeDone()
    {
        this.Func1 = () =>
        {
            var entityWithUnit = Contexts.sharedInstance.game.GetEntityWithUnit(this.Skill.UnitID);
            if (!entityWithUnit.hasInputKey)
            {
                return false;
            }


            int curNum;
            if (!entityWithUnit.inputKey.KeyCodes.TryGetValue(keyCode, out curNum))
            {
                return false;
            }

            if (Num == 1)
            {
                if (entityWithUnit.hasCombo)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (!entityWithUnit.hasCombo)
            {
                return false;
            }

            return entityWithUnit.combo.ComboIndex == Num - 1 && entityWithUnit.combo.PressedCount > 0 &&
                   entityWithUnit.combo.IsComboFinish;
        };
        return this.Func1;
    }
}