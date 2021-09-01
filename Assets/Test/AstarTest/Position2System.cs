using System.Collections.Generic;
using Entitas;
using UnityEngine;


public class Position2System : ReactiveSystem<GameEntity>
{
    public Position2System(Contexts context) : base(context.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Position2);
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity gameEntity in entities) 
        {
            Debug.Log(gameEntity.position2.X);
            gameEntity.ReplacePosition2(2,1,1);
        }
    }
}