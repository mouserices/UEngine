using System.Collections.Generic;
using Leopotam.Ecs;
using UEngine.UI.Runtime.Tools;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace UEngine.UI.Runtime.System
{
    public class UIConfigSystem : IEcsInitSystem
    {
        public void Init()
        {
            ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
            uiObjectComponent.UIConfigs = new Dictionary<string, UIConfig>();

            var uiConfigGO = ResKit.Load<GameObject>("UIConfig");
            var uiWindowConfig = uiConfigGO.GetComponent<UIWindowConfig>();
            foreach (var uiConfig in uiWindowConfig.UIConfigs)
            {
                if (!uiObjectComponent.UIConfigs.ContainsKey(uiConfig.NodeComponentName))
                {
                    uiObjectComponent.UIConfigs.Add(uiConfig.NodeComponentName, uiConfig);
                }
            }
        }
    }
}