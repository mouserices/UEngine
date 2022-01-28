using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UEngine.NP;
using UnityEngine;

public class NP_HitTargetConditionAction : NP_BaseAction
{
    [LabelText("子弹名称")]
    public NP_BlackBoardRelationData NPBalckBoardRelationData = new NP_BlackBoardRelationData();
    
    public override Func<bool> GetFunc1ToBeDone()
    {
        this.Func1 = () =>
        {
            // var entityWithUnit = Contexts.sharedInstance.game.GetEntityWithUnit(this.Skill.UnitID);
            // if (!entityWithUnit.hasHitTarget)
            // {
            //     return false;
            // }
            //
            // List<HitTarget> targets;
            // if (!entityWithUnit.hitTarget.Value.TryGetValue(this.Skill.Blackboard.Get<string>(NPBalckBoardRelationData.BBKey),out targets))
            // {
            //     return false;
            // }
            //
            // return targets.Exists(p => { return p.hasCalculateDamage == false;});
            
            return false;
        };
        return this.Func1;
    }
}