using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using Animancer;
using Sirenix.OdinInspector;
using UEngine.NP.FsmState;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace UEngine.NP.Unit
{
    //[RequireComponent(typeof(AnimancerComponent))]
    public abstract class BaseUnit : MonoBehaviour
    {
        // private AnimancerComponent m_AnimancerComponent;
        // // private EcsEntity m_Entity;
        //
        // [FormerlySerializedAs("NPBehaveName")] [ValueDropdown("GetAllNPBehaveName")]
        // public List<string> NPBehaveNames;
        //
        // public static List<string> GetAllNPBehaveName()
        // {
        //     var npBehaveConfigs =
        //         AssetDatabase.LoadAssetAtPath<NPBehaveConfigs>(
        //             "Assets/UEngine.NP/Config/Resources/NPBehaveConfigs.asset");
        //     return npBehaveConfigs.Configs.Keys.ToList();
        // }
        //
        // public void Awake()
        // {
        //     m_AnimancerComponent = this.GetComponent<AnimancerComponent>();
        // }

        public void Start()
        {
            // m_Entity = Game.EcsWorld.NewEntity();
            // m_Entity.Get<NP_TreeComponent>();
            // ref var wrapperUnityObjectComponent = ref m_Entity.Get<WrapperUnityObjectComponent<BaseUnit>>();
            // wrapperUnityObjectComponent.Value = this;
            // ref var requestRunNpEvent = ref m_Entity.Get<RequestRunNpEvent>();
            //
            // if (requestRunNpEvent.NPBehaves == null)
            // {
            //     requestRunNpEvent.NPBehaves = new List<string>();
            // }
            //
            // foreach (var behaveName in NPBehaveNames)
            // {
            //     requestRunNpEvent.NPBehaves.Add(behaveName);
            // }
            //
            // ref var animationComponent = ref m_Entity.Get<AnimationComponent>();
            // animationComponent.AnimancerComponent = m_AnimancerComponent;
            //
            // m_Entity.Get<FsmStateComponent>();
            // m_Entity.Get<InitFsmStateEvent>();
        }

        public void ChangeState(StateType stateType, string animClipName, int priority = 1,int durationTime = -1)
        {
            // ref var changeFsmStateEvent = ref m_Entity.Get<ChangeFsmStateEvent>();
            // if (changeFsmStateEvent.Processing)
            // {
            //     return;
            // }
            //
            // changeFsmStateEvent.Processing = true;
            // changeFsmStateEvent.StateType = stateType;
            // changeFsmStateEvent.AnimClipName = animClipName;
            // changeFsmStateEvent.Priority = priority;
            // changeFsmStateEvent.DurationTime = durationTime;
        }

        public void RemoveState(StateType stateType)
        {
            // ref var removeFsmStateEvent = ref m_Entity.Get<RemoveFsmStateEvent>();
            // removeFsmStateEvent.StateType = stateType;
        }

        public bool CheckState(StateType stateType)
        {
            // ref var fsmStateComponent = ref m_Entity.Get<FsmStateComponent>();
            // return fsmStateComponent.RuntimeFsmStateBases != null &&
            //        fsmStateComponent.RuntimeFsmStateBases.First != null &&
            //        fsmStateComponent.RuntimeFsmStateBases.First.Value.StateType == stateType;

            return true;
        }

        public void PlayAnim(string animClipName)
        {
            // ref var playAnimEvent = ref m_Entity.Get<PlayAnimEvent>();
            // playAnimEvent.AnimClipName = animClipName;
        }
    }
}