using System;
using Leopotam.Ecs;
using UnityEngine;

namespace Test.FilterTest
{
    public class FilterTest : MonoBehaviour
    {
        private EcsSystems _systems;

        public void Start()
        {
            EcsWorld _world = new EcsWorld();

            _systems = new EcsSystems(_world)
                    .Add(new FilterTest1())
                    .OneFrame<evt1>()
                ;
            _systems.Init();
        }

        public void Update()
        {
            _systems?.Run();
        }
    }

    public class FilterTestSystem : IEcsInitSystem, IEcsRunSystem
    {
        public EcsWorld _world = null;
        public EcsFilter<evt1,com1> _filter;
        public void Init()
        {
            var newEntity = _world.NewEntity();
            newEntity.Get<com1>();
            newEntity.Get<com2>();
            newEntity.Get<evt1>();
        }
        public void Run()
        {
            if (!_filter.IsEmpty())
            {
                Debug.Log(_filter.GetEntitiesCount());
            }
        }
    }

    public class FilterTest1 : FilterTestSystem
    {
        
    }

    public struct com1
    {
    }
    public struct com2
    {
    }
    public struct com3
    {
    }
    public struct evt1
    {
    }
    
}