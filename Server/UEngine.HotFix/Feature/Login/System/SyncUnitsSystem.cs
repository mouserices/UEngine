using System.Collections.Generic;
using Entitas;

public class SyncUnitsSystem : ReactiveSystem<GameEntity>
{
    public SyncUnitsSystem() : base(Contexts.sharedInstance.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.LoginMessage);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasLoginMessage;
    }

    protected override void Execute(List<GameEntity> entities)
    {
    }
}