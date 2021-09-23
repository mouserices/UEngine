using System.Collections.Generic;
using Entitas;

public class DamageSystem: ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    public DamageSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Damage);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasDamage;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity damageEntity in entities)
        {
            var damageTargetUnitID = damageEntity.damage.TargetUnitID;
            var damage = damageEntity.damage.Damage;

            var attackedEntity = _contexts.game.GetEntityWithUnit(damageTargetUnitID);
            if (attackedEntity == null)
            {
                continue;
            }

            if (attackedEntity.hasHP)
            {
                float maxHP = attackedEntity.hP.MaxHP;
                float curHP = attackedEntity.hP.CurHP - damage;
                attackedEntity.ReplaceHP(maxHP,curHP);
            }


#if LOCAL_SERVER
            //本地服务器的情况下，需要对镜像的实例做相同的处理
            var attackedEntityMirror = _contexts.remoteAgent.GetEntityWithUnit(damageTargetUnitID);
            if (attackedEntityMirror == null)
            {
                continue;
            }

            if (attackedEntityMirror.hasHP)
            {
                float maxHP = attackedEntityMirror.hP.MaxHP;
                float curHP = attackedEntityMirror.hP.CurHP - damage;
                attackedEntityMirror.ReplaceHP(maxHP,curHP);
            }
#endif
        }
    }
}