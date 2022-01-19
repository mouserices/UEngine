using System.Collections.Generic;
using Entitas;

public class HitNotifySystem : ReactiveSystem<UnitEntity>
{
    private readonly Contexts m_Contexts;

    public HitNotifySystem(Contexts contexts) : base(contexts.unit)
    {
        m_Contexts = contexts;
    }

    protected override ICollector<UnitEntity> GetTrigger(IContext<UnitEntity> context)
    {
        return context.CreateCollector(UnitMatcher.HitSucceed);
    }

    protected override bool Filter(UnitEntity entity)
    {
        return entity.isHitSucceed;
    }

    protected override void Execute(List<UnitEntity> entities)
    {
        foreach (UnitEntity bulletEntity in entities)
        {
            var bulletSourceUnitID = bulletEntity.bullet.SourceUnitID;
            var bulletSourceSkillID = bulletEntity.bullet.SourceSkillID;
            var name = bulletEntity.name.Value;
            var hitTargetsBlackBoardKey = bulletEntity.bindBlackBoardKeyToHitTargets.BlackBoardKey;
            var hitTargetTargets = bulletEntity.hitTarget.Targets;

            var hitTargets = hitTargetTargets.FindAll((p) => { return p.HasNotifyToServer == false; });
            List<long> attackedUnitIDs = new List<long>();
            foreach (HitTarget hitTarget in hitTargets)
            {
                hitTarget.HasNotifyToServer = true;
                attackedUnitIDs.Add(hitTarget.UnitID);
            }

            if (attackedUnitIDs.Count <= 0)
            {
                continue;
            }


#if LOCAL_SERVER
            // var remoteAgentEntity = m_Contexts.game.GetEntityWithUnit(bulletSourceUnitID + 10000);
            // var skillContainerSkills = remoteAgentEntity.skillContainer.Skills;
            //
            // var skill = skillContainerSkills.Find((p) => { return p.ID == bulletSourceSkillID;});
            // skill.Blackboard.Set<List<long>>(hitTargetsBlackBoardKey,attackedUnitIDs);
            // skill.Blackboard.Set<bool>(name,true);
#else

#endif
        }
    }
}