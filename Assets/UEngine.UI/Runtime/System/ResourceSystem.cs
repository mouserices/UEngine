using Leopotam.Ecs;
using UnityEngine;

class ResourceSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
{
    public EcsFilter<RequestLoadAssetEvent> Filter;

    public void PreInit()
    {
    }

    public void Init()
    {
    }

    public void Run()
    {
        // Will be called on each EcsSystems.Run() call.
        foreach (var i in Filter)
        {
            ref var requestLoadAssetEvent = ref Filter.Get1(i);
            var obj = Resources.Load(requestLoadAssetEvent.AssetName, requestLoadAssetEvent.AssetType);
            requestLoadAssetEvent.LoadCallBack(obj);
        }
    }
}