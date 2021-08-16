using Leopotam.Ecs;
using UEngine.UI.Event;

public class UIKit
{
    public static void OpenUI<T>() where T : struct
    {
        var uiEntity = Game.EcsWorld.NewEntity();
        uiEntity.Get<T>();

        ref var e = ref uiEntity.Get<RequestLoadUIEvent>();
        e.NodeComponentType = typeof(T);
    }

    public static void CloseUI<T>() where T : struct
    {
        ref var uiObjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        foreach (var uiEcsEntity in uiObjectComponent.UIEntities.Values)
        {
            if (uiEcsEntity.Has<T>())
            {
                uiEcsEntity.Get<UICloseEvent>();
                break;
            }
        }
    }
}