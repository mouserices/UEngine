using Leopotam.Ecs;
using UEngine.UI.Component;
using UnityEngine;

public class UITestSystem : UIBaseSystem<UITestNodeComponent>
{
    public override void Open(ref EcsEntity ecsEntity)
    {
        base.Open(ref ecsEntity);

        ref var nodeComponent = ref ecsEntity.Get<UITestNodeComponent>();
    }

    public override void Hide(ref EcsEntity ecsEntity)
    {
        base.Hide(ref ecsEntity);
    }

    public override void Freeze(ref EcsEntity ecsEntity)
    {
        base.Freeze(ref ecsEntity);
    }

    public override void ReShow(ref EcsEntity ecsEntity)
    {
        base.ReShow(ref ecsEntity);
    }
}