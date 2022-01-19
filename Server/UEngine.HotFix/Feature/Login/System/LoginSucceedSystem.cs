using System.Collections.Generic;
using Entitas;

/// <summary>
/// 登陆成功后的一些操作
/// 1。初始化session中其他的属性
/// </summary>
public class LoginSucceedSystem : ReactiveSystem<GameEntity>
{
    public LoginSucceedSystem() : base(Contexts.sharedInstance.game)
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