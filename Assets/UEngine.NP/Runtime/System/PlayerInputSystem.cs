using Leopotam.Ecs;
using NPBehave;
using StarterAssets;
using UEngine.NP.Component;
using UEngine.NP.Unit;

namespace UEngine.NP
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        private EcsFilter<WrapperUnityObjectComponent<BaseUnit>,NP_TreeComponent> m_Filter;
        public void Run()
        {
            foreach (var i in m_Filter)
            {
               ref var ecsEntity = ref m_Filter.GetEntity(i);
               ProcessPlayerInput(ecsEntity);
            }
        }

        private void ProcessPlayerInput(EcsEntity entity)
        {
            ref var wrapperUnityObjectComponent = ref entity.Get<WrapperUnityObjectComponent<BaseUnit>>();
            BaseUnit unit = wrapperUnityObjectComponent.Value;
            if (unit == null)
            {
                return;
            }
            var starterAssetsInputs = unit.GetComponent<StarterAssetsInputs>();
            if (starterAssetsInputs == null)
            {
                return;
            }

            ref var npTreeComponent = ref entity.Get<NP_TreeComponent>();
            foreach (Root root in npTreeComponent.Roots)
            {
                if (starterAssetsInputs.Keyboard_E)
                {
                    root.blackboard.Set<bool>("KeyboardInput_E",true);
                    starterAssetsInputs.Keyboard_E = false;
                }
            }
        }
    }
}