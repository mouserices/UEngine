using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ProcessInputSystem : ReactiveSystem<GameEntity>
{
    private Contexts m_Contexts;
    public ProcessInputSystem(Contexts contexts) : base(contexts.game)
    {
        m_Contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Input);
    }

    protected override bool Filter(GameEntity entity) => entity.hasInput;
    

    protected override void Execute(List<GameEntity> entities)
    {
        var singleEntity = entities.SingleEntity();
        var inputKeyCode = singleEntity.input.KeyCode;

        var gameMainPlayerEntity = m_Contexts.game.mainPlayerEntity;
        var behaveTreeComponent = gameMainPlayerEntity.behaveTree;

        foreach (var root in behaveTreeComponent.BehaveTreeRoots)
        {
            if (inputKeyCode == KeyCode.E)
            {
                root.blackboard.Set<bool>("KeyboardInput_E",true);
            }
        }
    }
}