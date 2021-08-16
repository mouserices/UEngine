using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UEngine.UI.Component;
using UEngine.UI.Event;
using UEngine.UI.Runtime.Tools;
using UnityEngine;
using Object = UnityEngine.Object;


public class UILoadSystem : IEcsInitSystem, IEcsRunSystem
{
    public EcsFilter<RequestLoadUIEvent> Filter;

    public void Init()
    {
        this.InitRootCanvas();
    }

    private void InitRootCanvas()
    {
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        uiObjectComponent.UIRootCanvas =
            GameObject.Instantiate(Resources.Load<GameObject>(UIDefine.SYS_PATH_CANVAS));

        uiObjectComponent.TransUIRoot = uiObjectComponent.UIRootCanvas.transform;
        uiObjectComponent.TransNormal = uiObjectComponent.TransUIRoot.Find("Normal");
        uiObjectComponent.TransFixed = uiObjectComponent.TransUIRoot.Find("Fixed");
        uiObjectComponent.TransPopUp = uiObjectComponent.TransUIRoot.Find("PopUp");

        uiObjectComponent.StackEntities = new Stack<EcsEntity>();
        uiObjectComponent.UIEntities = new Dictionary<string, EcsEntity>();
        uiObjectComponent.CurrentShowUIEntitys = new Dictionary<string, EcsEntity>();

        GameObject.DontDestroyOnLoad(uiObjectComponent.TransUIRoot);
    }

    public void Run()
    {
        foreach (var i in Filter)
        {
            ref var uiLoadRequestEvent = ref Filter.Get1(i);
            CheckAndLoad(ref uiLoadRequestEvent, ref Filter.GetEntity(i));
        }
    }

    private void CheckAndLoad(ref RequestLoadUIEvent uiLoadRequestEvent, ref EcsEntity ecsEntity)
    {
        if (!Check(ref uiLoadRequestEvent))
        {
            return;
        }

        var uiConfig = GetUIConfig(uiLoadRequestEvent.NodeComponentType.Name);
        AddUIEntity(uiConfig, ecsEntity);

        var gameObject = ResKit.Load<GameObject>(uiConfig.UIWindowName);
        OnLoadUIFinish(gameObject, uiConfig, ref ecsEntity);
    }

    private UIConfig GetUIConfig(string nodeComponentName)
    {
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        return uiObjectComponent.UIConfigs[nodeComponentName];
    }

    private bool Check(ref RequestLoadUIEvent uiLoadRequestEvent)
    {
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        if (!uiObjectComponent.UIConfigs.ContainsKey(uiLoadRequestEvent.NodeComponentType.Name))
        {
            return false;
        }

        var uiConfig = uiObjectComponent.UIConfigs[uiLoadRequestEvent.NodeComponentType.Name];

        if (uiConfig.UIWindowName == null)
        {
            return false;
        }

        if (uiObjectComponent.UIEntities == null)
        {
            return true;
        }
        return !uiObjectComponent.UIEntities.ContainsKey(uiConfig.UIWindowName);
    }

    private void OnLoadUIFinish(Object o, UIConfig uiConfig, ref EcsEntity ecsEntity)
    {
        var uiName = uiConfig.UIWindowName;
        var uiShowMode = uiConfig.UIShowMode;
        var uiLucenyType = uiConfig.UILucenyType;
        var uiType = uiConfig.UIType;

        var uiObj = ProcessUIType(o, uiType);
        ProcessShowMode(uiName,uiShowMode);
        //init Collector
        ref var referenceCollector = ref ecsEntity.Get<WrapperUnityObjectComponent<UIReferenceCollector>>();
        referenceCollector.Value = uiObj.GetComponent<UIReferenceCollector>();

        ref var uiInfoComponent = ref ecsEntity.Get<UIInfoComponent>();
        uiInfoComponent.Root = uiObj;
        uiInfoComponent.UIShowMode = uiShowMode;
        uiInfoComponent.UILucenyType = uiLucenyType;
        uiInfoComponent.UIType = uiType;
        uiInfoComponent.UIName = uiName;

        ecsEntity.Get<UIMaskEvent>().Visable = true;
        // notify open UI
        ecsEntity.Get<UIOpenEvent>();
    }

    private void ProcessShowMode(string uiName, UIShowMode uiShowMode)
    {
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        var ecsEntity = uiObjectComponent.UIEntities[uiName];

        switch (uiShowMode)
        {
            case UIShowMode.Normal:
                PushUIToCurrentShowCache(uiName, ref ecsEntity);
                break;
            case UIShowMode.ReverseChange:
                PushUIToStackCache(ref ecsEntity);
                break;
            case UIShowMode.HideOther:
                HideCurrentShowUI();
                PushUIToCurrentShowCache(uiName, ref ecsEntity);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(uiShowMode), uiShowMode, null);
        }
    }

    private void ProcessLucenyType(string uiName, UILucenyType uiLucenyType)
    {
    }

    private void PushUIToCurrentShowCache(string uiName, ref EcsEntity ecsEntity)
    {
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        if (uiObjectComponent.CurrentShowUIEntitys == null)
        {
            uiObjectComponent.CurrentShowUIEntitys = new Dictionary<string, EcsEntity>();
        }

        if (!uiObjectComponent.CurrentShowUIEntitys.ContainsKey(uiName))
        {
            uiObjectComponent.CurrentShowUIEntitys.Add(uiName, ecsEntity);
        }
    }

    private void PushUIToStackCache(ref EcsEntity ecsEntity)
    {
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        if (uiObjectComponent.StackEntities == null)
        {
            uiObjectComponent.StackEntities = new Stack<EcsEntity>();
        }

        if (uiObjectComponent.StackEntities.Count > 0)
        {
            var entity = uiObjectComponent.StackEntities.Peek();
            entity.Get<UIFreezeEvent>();
        }

        uiObjectComponent.StackEntities.Push(ecsEntity);
    }

    private void HideCurrentShowUI()
    {
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();

        foreach (var entity in uiObjectComponent.CurrentShowUIEntitys.Values)
        {
            entity.Get<UIHideEvent>();
        }

        foreach (var entity in uiObjectComponent.StackEntities)
        {
            entity.Get<UIHideEvent>();
        }
    }

    private void AddUIEntity(UIConfig uiConfig, EcsEntity ecsEntity)
    {
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        if (uiObjectComponent.UIEntities == null)
        {
            uiObjectComponent.UIEntities = new Dictionary<string, EcsEntity>();
        }

        if (!uiObjectComponent.UIEntities.ContainsKey(uiConfig.UIWindowName))
        {
            uiObjectComponent.UIEntities.Add(uiConfig.UIWindowName, ecsEntity);
        }
    }


    private GameObject ProcessUIType(Object o, UIType uiType)
    {
        var uiObj = GameObject.Instantiate(o) as GameObject;
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        //ui pos type
        switch (uiType)
        {
            case UIType.Normal: //普通窗体节点
                uiObj.transform.SetParent(uiObjectComponent.TransNormal, false);
                break;
            case UIType.Fixed: //固定窗体节点
                uiObj.transform.SetParent(uiObjectComponent.TransFixed, false);
                break;
            case UIType.PopUp: //弹出窗体节点
                uiObj.transform.SetParent(uiObjectComponent.TransPopUp, false);
                break;
            default:
                break;
        }

        return uiObj;
    }
}