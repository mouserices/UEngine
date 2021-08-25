using Animancer;
using Leopotam.Ecs;
using UEngine.NP.Component;
using UEngine.NP.Component.Event;
using UEngine.UI.Runtime.Tools;
using UnityEngine;

namespace UEngine.NP
{
    public class AnimationSystem : IEcsRunSystem
    {
        private EcsFilter<PlayAnimEvent> m_EcsFilter;

        public void Run()
        {
            foreach (int i in m_EcsFilter)
            {
                ref var ecsEntity = ref m_EcsFilter.GetEntity(i);
                PlayAnim(ref ecsEntity);
            }
        }

        private void PlayAnim(ref EcsEntity ecsEntity)
        {
            ref var playAnimEvent = ref ecsEntity.Get<PlayAnimEvent>();
            ref var animationComponent = ref ecsEntity.Get<AnimationComponent>();
            animationComponent.AnimancerComponent.Play(ResKit.Load<AnimationClip>(playAnimEvent.AnimClipName),0.25f,FadeMode.FromStart);
        }
    }
}