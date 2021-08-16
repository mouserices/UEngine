using System;
using Leopotam.Ecs;
using UEngine.UI.Component;
using UEngine.UI.Event;
using UnityEngine;

public class UICloseSystem : IEcsRunSystem
{
    private EcsFilter<UICloseEvent> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var entity = ref _filter.GetEntity(i);
            CloseUI(ref entity);
        }
    }

    private void CloseUI(ref EcsEntity ecsEntity)
    {
        if (!ecsEntity.Has<UIInfoComponent>())
        {
            return;
        }

        ref var uiInfoComponent = ref ecsEntity.Get<UIInfoComponent>();
        var uiName = uiInfoComponent.UIName;
        var uiShowMode = uiInfoComponent.UIShowMode;
        var root = uiInfoComponent.Root;

        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        if (!uiObjectComponent.UIEntities.ContainsKey(uiName))
        {
            return;
        }

        switch (uiShowMode)
        {
            case UIShowMode.Normal:
                CloseNormalUI(uiName, root, ref ecsEntity);
                break;
            case UIShowMode.ReverseChange:
                CloseUIAndPop(uiName, root, ref ecsEntity);
                break;
            case UIShowMode.HideOther:
                CloseUIAndOpenOther(uiName, root, ref ecsEntity);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CloseUIAndOpenOther(string uiName, GameObject root, ref EcsEntity ecsEntity)
    {
        //close 
        RemoveUIEntityCache(uiName);
        DestoryRoot(root);
        ecsEntity.Destroy();

        //open other
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();

        foreach (var entity in uiObjectComponent.CurrentShowUIEntitys.Values)
        {
            entity.Get<UIShowEvent>();
        }

        foreach (var entity in uiObjectComponent.StackEntities)
        {
            entity.Get<UIShowEvent>();
        }
    }

    private void CloseUIAndPop(string uiName, GameObject root, ref EcsEntity ecsEntity)
    {
        //close 
        RemoveUIEntityCache(uiName);
        DestoryRoot(root);
        ecsEntity.Destroy();

        //pop
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        if (uiObjectComponent.StackEntities.Count >= 2)
        {
            uiObjectComponent.StackEntities.Pop();
            var entity = uiObjectComponent.StackEntities.Peek();

            entity.Get<UIMaskEvent>().Visable = true;
        }
        else if (uiObjectComponent.StackEntities.Count == 1)
        {
            uiObjectComponent.StackEntities.Pop();
            Game.EcsWorld.SendMessage(new UIMaskEvent(){Visable = false});
        }
    }

    private void CloseNormalUI(string uiName, GameObject root, ref EcsEntity ecsEntity)
    {
        RemoveUIEntityCache(uiName);
        DestoryRoot(root);
        ecsEntity.Destroy();
    }

    private void RemoveUIEntityCache(string uiName)
    {
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        if (uiObjectComponent.UIEntities.ContainsKey(uiName))
        {
            uiObjectComponent.UIEntities.Remove(uiName);
        }
        
        if (uiObjectComponent.CurrentShowUIEntitys.ContainsKey(uiName))
        {
            uiObjectComponent.CurrentShowUIEntitys.Remove(uiName);
        }
    }

    private void DestoryRoot(GameObject uiRoot)
    {
        GameObject.Destroy(uiRoot);
    }
}