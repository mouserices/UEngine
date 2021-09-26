using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class HitNotifySystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts m_Contexts;

    public HitNotifySystem(Contexts contexts) : base(contexts.game)
    {
        m_Contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.HitSucceed);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isHitSucceed;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity bulletEntity in entities)
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
                Debug.Log($"HitNotifySucceed:{hitTarget.UnitID}");
            }

            if (attackedUnitIDs.Count <= 0)
            {
                continue;
            }


#if LOCAL_SERVER
            Debug.Log("LOCAL_SERVER");
            var remoteAgentEntity = m_Contexts.game.GetEntityWithUnit(bulletSourceUnitID + 10000);
            var skillContainerSkills = remoteAgentEntity.skillContainer.Skills;

            var skill = skillContainerSkills.Find((p) => { return p.ID == bulletSourceSkillID;});
            skill.Blackboard.Set<List<long>>(hitTargetsBlackBoardKey,attackedUnitIDs);
            skill.Blackboard.Set<bool>(name,true);
#else

#endif
        }
    }
}