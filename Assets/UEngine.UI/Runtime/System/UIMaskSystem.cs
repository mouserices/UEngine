using System;
using Leopotam.Ecs;
using UEngine.UI.Component;
using UEngine.UI.Event;
using UnityEngine;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace UEngine.UI.Runtime.System
{
    public class UIMaskSystem : IEcsInitSystem,IEcsRunSystem
    {
        private EcsFilter<UIMaskEvent> _Filter;
        public void Init()
        {
            ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
            uiObjectComponent.TransMask = uiObjectComponent.TransPopUp.Find("Mask");
            uiObjectComponent.TransMask.gameObject.SetActive(false);
        }

        public void Run()
        {
            foreach (var i in _Filter)
            {
                DoMask( ref _Filter.Get1(i),ref _Filter.GetEntity(i));
            }
        }

        private void DoMask(ref UIMaskEvent uiMaskEvent,ref EcsEntity ecsEntity)
        {
            ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
            if (uiMaskEvent.Visable == false)
            {
                uiObjectComponent.TransMask.gameObject.SetActive(false);
                return;
            }
            
            if (!ecsEntity.Has<UIInfoComponent>())
            {
                return;
            }

            ref var uiInfoComponent = ref ecsEntity.Get<UIInfoComponent>();
            var root = uiInfoComponent.Root;
            var uiType = uiInfoComponent.UIType;
            var uiShowMode = uiInfoComponent.UIShowMode;
            var uiLucenyType = uiInfoComponent.UILucenyType;
            if (uiType != UIType.PopUp || uiShowMode != UIShowMode.ReverseChange)
            {
                return;
            }

            

            switch (uiLucenyType)
            {
                case UILucenyType.Lucency:
                    uiObjectComponent.TransMask.gameObject.SetActive(true);
                    Color newColor1 = new Color(UIDefine.SYS_UIMASK_LUCENCY_COLOR_RGB, UIDefine.SYS_UIMASK_LUCENCY_COLOR_RGB, UIDefine.SYS_UIMASK_LUCENCY_COLOR_RGB, UIDefine.SYS_UIMASK_LUCENCY_COLOR_RGB_A);
                    uiObjectComponent.TransMask.GetComponent<Image>().color = newColor1; 
                    break;
                case UILucenyType.Translucence:
                    uiObjectComponent.TransMask.gameObject.SetActive(true);
                    Color newColor2 = new Color(UIDefine.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB, UIDefine.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB, UIDefine.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB, UIDefine.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB_A);
                    uiObjectComponent.TransMask.GetComponent<Image>().color = newColor2; 
                    break;
                case UILucenyType.ImPenetrable:
                    uiObjectComponent.TransMask.gameObject.SetActive(true);
                    Color newColor3 = new Color(UIDefine.SYS_UIMASK_IMPENETRABLE_COLOR_RGB, UIDefine.SYS_UIMASK_IMPENETRABLE_COLOR_RGB, UIDefine.SYS_UIMASK_IMPENETRABLE_COLOR_RGB, UIDefine.SYS_UIMASK_IMPENETRABLE_COLOR_RGB_A);
                    uiObjectComponent.TransMask.GetComponent<Image>().color = newColor3; 
                    break;
                case UILucenyType.Pentrate:
                    if (uiObjectComponent.TransMask.gameObject.activeInHierarchy)
                    {
                        uiObjectComponent.TransMask.gameObject.SetActive(false);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            uiObjectComponent.TransMask.SetAsLastSibling();
            root.transform.SetAsLastSibling();
        }
    }
}