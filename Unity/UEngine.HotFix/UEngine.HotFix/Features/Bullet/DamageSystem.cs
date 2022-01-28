using System.Collections.Generic;
using Entitas;

public class DamageSystem : ReactiveSystem<UnitEntity>
{
    private readonly Contexts _contexts;

    public DamageSystem(Contexts contexts) : base(contexts.unit)
    {
        _contexts = contexts;
    }

    protected override ICollector<UnitEntity> GetTrigger(IContext<UnitEntity> context)
    {
        return context.CreateCollector(UnitMatcher.Damage);
    }

    protected override bool Filter(UnitEntity entity)
    {
        return entity.hasDamage;
    }

    protected override void Execute(List<UnitEntity> entities)
    {
        foreach (UnitEntity damageEntity in entities)
        {
            var damageTargetUnitID = damageEntity.damage.TargetUnitID;
            var damage = damageEntity.damage.Damage;

            var attackedEntity = _contexts.unit.GetEntityWithUnit(damageTargetUnitID);
            if (attackedEntity == null)
            {
                continue;
            }
            
            attackedEntity.ChangeNumeric(NumericType.HP, -damage);


// #if LOCAL_SERVER
//             //本地服务器的情况下，需要对镜像的实例做相同的处理
//             var attackedEntityMirror = _contexts.game.GetEntityWithUnit(damageTargetUnitID + 10000);
//             if (attackedEntityMirror == null)
//             {
//                 continue;
//             }
//
//             attackedEntityMirror.ChangeNumeric(NumericType.HP, -damage);
// #endif
        }
    }
}