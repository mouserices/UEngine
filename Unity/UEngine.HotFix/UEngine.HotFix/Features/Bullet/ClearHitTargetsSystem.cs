using System.Collections.Generic;
using Entitas;

public class ClearHitTargetsSystem: ReactiveSystem<UnitEntity>
{
    private readonly Contexts _contexts;
    public ClearHitTargetsSystem(Contexts contexts) : base(contexts.unit)
    {
        _contexts = contexts;
    }

    protected override ICollector<UnitEntity> GetTrigger(IContext<UnitEntity> context)
    {
        return context.CreateCollector(UnitMatcher.AllOf(UnitMatcher.Bullet,UnitMatcher.Destroyed));
    }

    protected override bool Filter(UnitEntity entity)
    {
        return entity.hasBullet && entity.isDestroyed;
    }

    protected override void Execute(List<UnitEntity> entities)
    {
        // foreach (GameEntity bulletEntity in entities)
        // {
        //     var bulletSourceUnitID = bulletEntity.bullet.SourceUnitID;
        //     var attackEntity = _contexts.game.GetEntityWithUnit(bulletSourceUnitID);
        //     if (attackEntity.hasHitTarget)
        //     {
        //         if (attackEntity.hitTarget.Value.ContainsKey(bulletEntity.name.Value))
        //         {
        //             attackEntity.hitTarget.Value.Remove(bulletEntity.name.Value);
        //         }
        //     }
        // }
    }
}