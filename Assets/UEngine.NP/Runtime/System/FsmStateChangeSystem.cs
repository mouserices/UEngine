using System.Collections.Generic;
using System.Security.Policy;
using Leopotam.Ecs;
using UEngine.NP.Component;
using UEngine.NP.Component.Event;
using UEngine.NP.FsmState;
using UEngine.NP.Unit;
using UnityEngine.XR;

namespace UEngine.NP
{
    public class FsmStateChangeSystem : IEcsRunSystem
    {
        private EcsFilter<ChangeFsmStateEvent, WrapperUnityObjectComponent<BaseUnit>> m_EcsFilter;
        private EcsFilter<RemoveFsmStateEvent> m_EcsFilter2;

        public void Run()
        {
            foreach (var i in m_EcsFilter)
            {
                ref var ecsEntity = ref m_EcsFilter.GetEntity(i);
                ChangeState(ref ecsEntity);
            }
            
            foreach (var i in m_EcsFilter2)
            {
                ref var ecsEntity = ref m_EcsFilter2.GetEntity(i);
                RemoveState(ref ecsEntity);
            }
        }

        private void RemoveState(ref EcsEntity ecsEntity)
        {
            ref var removeFsmStateEvent = ref ecsEntity.Get<RemoveFsmStateEvent>();
            var stateType = removeFsmStateEvent.StateType;

            ref var fsmStateComponent = ref ecsEntity.Get<FsmStateComponent>();
            FsmStateBase fsmStateBaseToRemove = null;
            foreach (FsmStateBase runtimeFsmStateBase in fsmStateComponent.RuntimeFsmStateBases)
            {
                if (runtimeFsmStateBase.StateType == stateType)
                {
                    fsmStateBaseToRemove = runtimeFsmStateBase;
                    break;
                }
            }

            bool isFirstState = false;
            if (fsmStateComponent.RuntimeFsmStateBases.First != null)
            {
                isFirstState = fsmStateComponent.RuntimeFsmStateBases.First.Value.StateType == stateType;
            }
            if (fsmStateBaseToRemove != null)
            {
                fsmStateBaseToRemove.OnRemove();
                fsmStateComponent.RuntimeFsmStateBases.Remove(fsmStateBaseToRemove);
            }

            if (isFirstState)
            {
                fsmStateComponent.RuntimeFsmStateBases.First?.Value.OnEnter();
            }
        }

        private void ChangeState(ref EcsEntity ecsEntity)
        {
            ref var changeStateEvent = ref ecsEntity.Get<ChangeFsmStateEvent>();
            var stateType = changeStateEvent.StateType;
            ref var fsmStateComponent = ref ecsEntity.Get<FsmStateComponent>();
            var animClipName = changeStateEvent.AnimClipName;

            insterState(ref ecsEntity, ref changeStateEvent);

            changeStateEvent.Processing = false;
        }

        private void insterState(ref EcsEntity ecsEntity, ref ChangeFsmStateEvent changeStateEvent)
        {
            var stateType = changeStateEvent.StateType;
            ref var fsmStateComponent = ref ecsEntity.Get<FsmStateComponent>();
            var containsKey = fsmStateComponent.FsmStateBases.ContainsKey(stateType);
            if (!containsKey)
            {
                return;
            }

            var firstState = fsmStateComponent.RuntimeFsmStateBases.First;

            if (firstState != null && firstState.Value.StateType == stateType)
            {
                return;
            }

            //状态冲突处理
            var fsmStateBaseTarget = fsmStateComponent.FsmStateBases[stateType];
            fsmStateBaseTarget.DurationTime = changeStateEvent.DurationTime;
            fsmStateBaseTarget.BaseUnit = ecsEntity.Get<WrapperUnityObjectComponent<BaseUnit>>().Value;
            fsmStateBaseTarget.AnimClipName = changeStateEvent.AnimClipName;
            
            if (firstState != null && !fsmStateBaseTarget.TryEnter(firstState.Value.StateType))
            {
                return;
            }

            //寻找相同的优先级
            LinkedListNode<FsmStateBase> current = firstState;
            while (current != null)
            {
                if (fsmStateBaseTarget.Priority >= current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }


            bool isContainerSelf = false;
            foreach (FsmStateBase fsmStateBase in fsmStateComponent.RuntimeFsmStateBases)
            {
                if (fsmStateBase.StateType == stateType)
                {
                    isContainerSelf = true;
                    break;
                }
            }

            //如果自身包含了目标状态，则把目标状态放在相同优先级的前面
            if (isContainerSelf)
            {
                fsmStateComponent.RuntimeFsmStateBases.Remove(fsmStateBaseTarget);
                fsmStateComponent.RuntimeFsmStateBases.AddBefore(current, fsmStateBaseTarget);
            }
            else
            {
                if (current == null)
                {
                    fsmStateComponent.RuntimeFsmStateBases.AddLast(fsmStateBaseTarget);
                }
                else
                {
                    fsmStateComponent.RuntimeFsmStateBases.AddBefore(current, fsmStateBaseTarget);
                }
            }


            var b = fsmStateComponent.RuntimeFsmStateBases.First.Value.StateType == stateType;
            if (b)
            {
                firstState?.Value.OnExist();
                fsmStateBaseTarget.OnEnter();
            }
        }
    }
}