using System;
using Leopotam.Ecs;
using UEngine.UI.Component;
using NotImplementedException = System.NotImplementedException;

namespace UEngine.UI.Runtime.System
{
    public class AutoInjectSystem : IEcsInitSystem
    {
        public void Init()
        {
            ref var attributeComponent = ref Game.MainEntity.Get<AttributeComponent>();
            if (attributeComponent.AutoInjectSystems != null)
            {
                foreach (var autoInjectSystem in attributeComponent.AutoInjectSystems)
                {
                    var instance = Activator.CreateInstance(autoInjectSystem);
                    IEcsSystem ecsInitSystem = instance as IEcsSystem;
                    Game.EcsSystems.Add(ecsInitSystem);
                }
            }
        }
    }
}