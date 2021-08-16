using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Leopotam.Ecs;
using UEngine.UI.Event;
using UnityEngine;

public class UIBaseSystem<T> : IEcsRunSystem where T : struct
{
    public EcsFilter<UIOpenEvent, T> FilterT1;
    public EcsFilter<UIHideEvent, T> FilterT2;
    public EcsFilter<UIShowEvent, T> FilterT3;
    public EcsFilter<UIFreezeEvent, T> FilterT4;

    public void Run()
    {
        if (!FilterT1.IsEmpty())
        {
            var entity = FilterT1.GetEntity(0);
            AutoInject(ref entity);
            BindTypeToEntity(typeof(T), ref entity);
            Open(ref entity);
        }

        if (!FilterT2.IsEmpty())
        {
            var entity = FilterT2.GetEntity(0);
            Hide(ref entity);
        }
        if (!FilterT3.IsEmpty())
        {
            var entity = FilterT3.GetEntity(0);
            ReShow(ref entity);
        }
        if (!FilterT4.IsEmpty())
        {
            var entity = FilterT4.GetEntity(0);
            Freeze(ref entity);
        }
    }
    public virtual void Open(ref EcsEntity ecsEntity)
    {
    }

    public virtual void Hide(ref EcsEntity ecsEntity)
    {
    }
    public virtual void ReShow(ref EcsEntity ecsEntity)
    {
    }

    public virtual void Freeze(ref EcsEntity ecsEntity)
    {
    }

    private void BindTypeToEntity(Type t, ref EcsEntity ecsEntity)
    {
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        if (uiObjectComponent.UITypeToEntities == null)
        {
            uiObjectComponent.UITypeToEntities = new Dictionary<Type, EcsEntity>();
        }

        if (!uiObjectComponent.UITypeToEntities.ContainsKey(t))
        {
            uiObjectComponent.UITypeToEntities.Add(t, ecsEntity);
        }
    }

    private void AutoInject(ref EcsEntity ecsEntity)
    {
        ref var uiComponent = ref ecsEntity.Get<T>();
        object uiComponentObj;

        if (Game.MainEntity.Get<UIObjectComponent>().UINodeObjects == null)
        {
            Game.MainEntity.Get<UIObjectComponent>().UINodeObjects = new Dictionary<Type, object>();
        }

        if (!Game.MainEntity.Get<UIObjectComponent>().UINodeObjects.TryGetValue(typeof(T), out uiComponentObj))
        {
            uiComponentObj = uiComponent;
            Game.MainEntity.Get<UIObjectComponent>().UINodeObjects.Add(typeof(T), uiComponentObj);
        }

        ref var uiNodeInjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        var fieldInfos = uiNodeInjectComponent.FieldInfos[typeof(T)];
        foreach (var fieldInfo in fieldInfos)
        {
            var obj =
                ecsEntity.Get<WrapperUnityObjectComponent<UIReferenceCollector>>().Value
                    .GetObject(fieldInfo.Name);
            fieldInfo.SetValue(uiComponentObj, obj);
        }

        var componentObj = (T) uiComponentObj;
        ecsEntity.Replace<T>(componentObj);
    }
}