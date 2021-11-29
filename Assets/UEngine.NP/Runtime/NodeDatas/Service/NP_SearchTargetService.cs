using System;
using System.Linq;
using Sirenix.OdinInspector;
using UEngine.NP;

public class NP_SearchTargetService : NP_BaseService
{
    [LabelText("搜寻到的目标存入指定的黑板中")] public NP_BlackBoardKeySelecter<NP_BBValue_Long> BlackBoard_TargetUnitID =
        new NP_BlackBoardKeySelecter<NP_BBValue_Long>();

    [LabelText("距离目标的距离")] public NP_BlackBoardKeySelecter<NP_BBValue_Float> BlackBoard_TargetDistance =
        new NP_BlackBoardKeySelecter<NP_BBValue_Float>();

    public override Action GetServiceAction()
    {
        return new Action(() =>
        {
            var campType = CampType.Enemy;
            var entity = Contexts.sharedInstance.game.GetEntityWithUnit(this.Skill.UnitID);
            if (entity.camp.Value == CampType.Enemy)
            {
                campType = CampType.Own;
            }

            var entitiesWithCamp = Contexts.sharedInstance.game.GetEntitiesWithCamp(campType);
            if (entitiesWithCamp != null && entitiesWithCamp.Count > 0)
            {
                var targetEntity = entitiesWithCamp.First();
                this.Skill.Root.blackboard.Set<long>(BlackBoard_TargetUnitID.BBKey, targetEntity.unit.ID);

                var positionValue = targetEntity.position.value - entity.position.value;
                this.Skill.Root.blackboard.Set<float>(BlackBoard_TargetDistance.BBKey, positionValue.magnitude);
            }
        });
    }
}