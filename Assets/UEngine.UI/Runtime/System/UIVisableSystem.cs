using Leopotam.Ecs;
using UEngine.UI.Component;
using UEngine.UI.Event;
using UnityEngine;

public class UIVisableSystem : IEcsRunSystem
{
    private EcsFilter<UIHideEvent> _HideFilter;
    private EcsFilter<UIShowEvent> _ShowFilter;
    public void Run()
    {
        foreach (var i in _HideFilter)
        {
            var ecsEntity = _HideFilter.GetEntity(i);
            ref var visalbeComponent = ref ecsEntity.Get<UIInfoComponent>();
            visalbeComponent.Root.SetActive(false);
        }

        foreach (var i in _ShowFilter)
        {
            var ecsEntity = _ShowFilter.GetEntity(i);
            ref var visalbeComponent = ref ecsEntity.Get<UIInfoComponent>();
            visalbeComponent.Root.SetActive(true);
        }
    }
}