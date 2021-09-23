using System.Collections.Generic;
using Entitas;

public class ClearHitTargetsSystem: ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    public ClearHitTargetsSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Bullet,GameMatcher.Destroyed));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasBullet && entity.isDestroyed;
    }

    protected override void Execute(List<GameEntity> entities)
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