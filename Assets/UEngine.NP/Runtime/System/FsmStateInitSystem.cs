using System.Collections.Generic;
using Leopotam.Ecs;
using UEngine.NP.Component;
using UEngine.NP.Component.Event;
using UEngine.NP.FsmState;
using UnityEngine;

namespace UEngine.NP
{
    public class FsmStateInitSystem : IEcsRunSystem
    {
        private EcsFilter<InitFsmStateEvent> m_EcsFilter;

        public void Run()
        {
            foreach (int i in m_EcsFilter)
            {
                ref var ecsEntity = ref m_EcsFilter.GetEntity(i);
                InitFsmState(ref ecsEntity);
            }
        }

        private void InitFsmState(ref EcsEntity ecsEntity)
        {
           ref var fsmStateComponent = ref ecsEntity.Get<FsmStateComponent>();
           if (fsmStateComponent.FsmStateBases == null)
           {
               fsmStateComponent.FsmStateBases = new Dictionary<StateType, FsmStateBase>();
           }
           
           if (fsmStateComponent.RuntimeFsmStateBases == null)
           {
               fsmStateComponent.RuntimeFsmStateBases = new LinkedList<FsmStateBase>();
           }

           RegisterState(fsmStateComponent.FsmStateBases,StateType.IDLE,new IdleState());
           RegisterState(fsmStateComponent.FsmStateBases,StateType.Run,new RunState());
           RegisterState(fsmStateComponent.FsmStateBases,StateType.Walk,new WalkState());
           RegisterState(fsmStateComponent.FsmStateBases,StateType.Attack,new AttackState());
        }

        private void RegisterState(Dictionary<StateType,FsmStateBase> fsmStateBases,StateType stateType,FsmStateBase fsmStateBase)
        {
            if (!fsmStateBases.ContainsKey(stateType))
            {
                fsmStateBase.StateType = stateType;
                fsmStateBases.Add(stateType,fsmStateBase);
            }
            else
            {
                Debug.LogError("RegisterState Error, same key");
            }
        }
    }
}