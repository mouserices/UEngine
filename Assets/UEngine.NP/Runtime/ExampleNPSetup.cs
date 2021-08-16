using System;
using Leopotam.Ecs;
using UEngine.NP.Component;
using UEngine.NP.Component.Event;
using UnityEngine;

namespace UEngine.NP
{
    public class ExampleNPSetup : MonoBehaviour
    {
        public void Start()
        {
            EcsWorld _world = new EcsWorld();
            EcsSystems _systems = new EcsSystems(_world)
                .Add(new NP_TreeDataSystem())
                .Add(new NP_TreeFactorySystem())
                
                .OneFrame<RequestRunNpEvent>()
                ;

            Game.EcsWorld = _world;
            Game.EcsSystems = _systems;
            Game.MainEntity = _world.NewEntity();
            Game.EcsSystems.Init();
            Test();
        }

        private void Test()
        {
            var newEntity = Game.EcsWorld.NewEntity();
            newEntity.Get<NP_TreeComponent>();
            ref var requestRunNpEvent = ref newEntity.Get<RequestRunNpEvent>();
            requestRunNpEvent.NP_TreeName = "NPBehaveGraph";
        }

        public void Update()
        {
            Game.EcsSystems?.Run();
        }
    }
}