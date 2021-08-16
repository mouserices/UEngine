using Leopotam.Ecs;
using UEngine.UI.Component;
using UnityEngine;

[AutoInjectSystem]
public class UIEquipSystem : UIBaseSystem<UIEquipNodeComponent>
{
    public override void Open(ref EcsEntity ecsEntity)
    {
        base.Open(ref ecsEntity);
        ref var uiEquipNodeComponent = ref ecsEntity.Get<UIEquipNodeComponent>();
        uiEquipNodeComponent.Title.text = "我是装备面板";

        uiEquipNodeComponent.BtnClose.onClick.AddListener(() => { UIKit.CloseUI<UIEquipNodeComponent>(); });
    }

    public override void Hide(ref EcsEntity ecsEntity)
    {
        base.Hide(ref ecsEntity);
        Debug.Log("我是装备面板，我被隐藏了- OnHide");
    }

    public override void Freeze(ref EcsEntity ecsEntity)
    {
        base.Freeze(ref ecsEntity);
        Debug.Log("我是装备面板，我被冻结了- OnFreeze");
    }

    public override void ReShow(ref EcsEntity ecsEntity)
    {
        base.ReShow(ref ecsEntity);
        Debug.Log("我是装备面板，我又被显示了- ReShow");
    }
}