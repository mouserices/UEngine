using NPBehave;
using Sirenix.OdinInspector;
using UEngine.NP;
using UnityEngine;

public class NP_MoveToTargetAction : NP_BaseAction
{
    [LabelText("搜寻到的目标存入指定的黑板中")] public NP_BlackBoardKeySelecter<NP_BBValue_Long> BlackBoard_TargetUnitID =
        new NP_BlackBoardKeySelecter<NP_BBValue_Long>();
    public override System.Func<bool, Action.Result> GetFunc2ToBeDone()
    {
        this.Func2 = (b) =>
        {
            var targetUnitID = this.Skill.Blackboard.Get<long>(BlackBoard_TargetUnitID.BBKey);
            if (targetUnitID > 0)
            {
                var entity = Contexts.sharedInstance.unit.GetEntityWithUnit(this.UnitID);
                var targetEntity = Contexts.sharedInstance.unit.GetEntityWithUnit(targetUnitID);
                if (!b)
                {
                    var moveDir = targetEntity.position.value - entity.position.value;
                    var dir = new Vector2(moveDir.x, moveDir.z);
                    entity.ReplaceMove(true, dir, 0.2f);
                    return NPBehave.Action.Result.PROGRESS;
                }
                else
                {
                    if (entity.move.IsMoving)
                    {
                        entity.ReplaceMove(false, Vector2.zero, 0.2f);
                    }
                }
            }

            return NPBehave.Action.Result.FAILED;
        };
        return this.Func2;
    }
}