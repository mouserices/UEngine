using System;
using Leopotam.Ecs;
using UEngine.NP.Component;
using UEngine.NP.Component.Event;
using UnityEngine;

namespace UEngine.NP
{
    public class ExampleNPSetup : MonoBehaviour
    {
        public void Awake()
        {
            EcsWorld _world = new EcsWorld();
            EcsSystems _systems = new EcsSystems(_world)
                .Add(new NP_TreeDataSystem())
                .Add(new NP_TreeFactorySystem())
                .Add(new PlayerInputSystem())
                .Add(new FsmStateInitSystem())
                .Add(new FsmStateChangeSystem())
                .Add(new AnimationSystem())
                
                .OneFrame<RequestRunNpEvent>()
                .OneFrame<InitFsmStateEvent>()
                .OneFrame<ChangeFsmStateEvent>()
                .OneFrame<PlayAnimEvent>()
                .OneFrame<RemoveFsmStateEvent>()
                ;

            Game.EcsWorld = _world;
            Game.EcsSystems = _systems;
            Game.MainEntity = _world.NewEntity();
            Game.EcsSystems.Init();
        }
        public void Update()
        {
            Game.EcsSystems?.Run();
        }
    }
}