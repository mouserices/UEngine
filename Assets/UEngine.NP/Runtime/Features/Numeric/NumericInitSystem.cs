using System.Collections.Generic;
using Entitas;

/// <summary>
/// 1.初始化每个角色的基础数值、hp、maxhp、attack
/// 2.目前先写死数值
/// 3.todo 后期考虑在这里根据config读取基础数值
/// </summary>
public class NumericInitSystem : ReactiveSystem<GameEntity>
{
    private Contexts _Contexts;
    public NumericInitSystem(Contexts context) : base(context.game)
    {
        _Contexts = context;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.NumericModifier);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasNumeric && entity.hasNumericModifier;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity playerEntity in entities)
        {
            playerEntity.AddNumericModifier(NumericType.HP,new FixedModifier(){FixedValue = 100});
            playerEntity.CalculateNumeric(NumericType.HP);
            
            playerEntity.AddNumericModifier(NumericType.MAX_HP,new FixedModifier(){FixedValue = 100});
            playerEntity.CalculateNumeric(NumericType.MAX_HP);
            
            playerEntity.AddNumericModifier(NumericType.ATTACK,new FixedModifier(){FixedValue = 10});
            playerEntity.CalculateNumeric(NumericType.ATTACK);
        }
    }
}