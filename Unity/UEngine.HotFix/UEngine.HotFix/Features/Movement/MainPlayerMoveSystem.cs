using Entitas;
using UnityEngine;

public class MainPlayerMoveSystem : ITickSystem
{
    private readonly Contexts _contexts;

    public MainPlayerMoveSystem(Contexts contexts)
    {
        _contexts = contexts;
    }
    public void Tick()
    {
        var mainPlayerEntity = _contexts.unit.mainPlayerEntity;
        if (mainPlayerEntity == null)
        {
            return;
        }
        var moveDir = MetaContext.Get<IInputService>().GetMoveDir();
        mainPlayerEntity.ReplaceMove(moveDir != Vector2.zero, moveDir, 1f);
    }
}