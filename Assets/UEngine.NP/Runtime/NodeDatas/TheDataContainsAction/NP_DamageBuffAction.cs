using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UEngine.NP;
using UnityEngine;

public class NP_DamageBuffAction : NP_ClassForStoreAction
{
    [LabelText("目标来源")]
    public NP_BlackBoardKeySelecter<NP_BBValue_List_Long> BlackBoard_TargetUnitIDs = new NP_BlackBoardKeySelecter<NP_BBValue_List_Long>();

    [LabelText("伤害量")]
    public float Damage;
    public override Action GetActionToBeDone()
    {
        this.Action = () =>
        {
            List<long> targetUnitIDs = this.Skill.Blackboard.Get<List<long>>(BlackBoard_TargetUnitIDs.BBKey);

            foreach (long unitID in targetUnitIDs)
            {
                var damageEntity = Contexts.sharedInstance.game.CreateEntity();
                damageEntity.AddDamage(unitID,Damage);
            }
        };
        return this.Action;
    }
}