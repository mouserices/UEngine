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
        if (entities.Count > 1)
        {
            return;
        }
        var singleEntity = entities.SingleEntity();
        var inputKeyCode = singleEntity.input.KeyCode;

        var gameMainPlayerEntity = m_Contexts.game.mainPlayerEntity;
        var inputKeyKeyCodes = gameMainPlayerEntity.inputKey.KeyCodes;
        int num;
        if (!inputKeyKeyCodes.TryGetValue(inputKeyCode,out num))
        {
            inputKeyKeyCodes.Add(inputKeyCode,0);
        }

        inputKeyKeyCodes[inputKeyCode] = num + 1;
        if (inputKeyCode == KeyCode.E)
        {
            if (gameMainPlayerEntity.hasCombo)
            {
                gameMainPlayerEntity.combo.PressedCount = gameMainPlayerEntity.combo.PressedCount + 1;
            }
        }

        // var skills = gameMainPlayerEntity.skillContainer.Skills;
        //
        // foreach (var skill in skills)
        // {
        //     if (inputKeyCode == KeyCode.E)
        //     {
        //         skill.Blackboard.Set<bool>("KeyboardInput_E",true);
        //     }
        // }
    }
}