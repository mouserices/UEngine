using System;
using Sirenix.OdinInspector;
using UEngine.NP;
using UnityEngine;

public class NP_IsConditionMetOfInputKey : NP_BaseAction
{
    [LabelText("监听的哪个Key")]
    public KeyCode keyCode;
    [LabelText("监听按下的次数")]
    public int Num;
    
    public override Func<bool> GetFunc1ToBeDone()
    {
        this.Func1 = () =>
        {
            var entityWithUnit = Contexts.sharedInstance.unit.GetEntityWithUnit(this.Skill.UnitID);
            if (!entityWithUnit.hasInputKey)
            {
                return false;
            }
            
            int curNum;
            if (!entityWithUnit.inputKey.KeyCodes.TryGetValue(keyCode,out curNum))
            {
                return false;
            }
            
            return Num <= curNum;
            // return true;
        };
        return this.Func1;
    }
}