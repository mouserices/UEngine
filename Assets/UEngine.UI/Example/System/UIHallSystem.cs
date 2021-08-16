using Leopotam.Ecs;
using UEngine.UI.Component;
using UnityEngine;

[AutoInjectSystem]
public class UIHallSystem : UIBaseSystem<UIHallNodeComponent>
{
    public override void Open(ref EcsEntity ecsEntity)
    {
        base.Open(ref ecsEntity);
        Debug.Log("我是大厅面板，我第一次被打开- OnOpen");


        ref var uiHallNodeComponent = ref ecsEntity.Get<UIHallNodeComponent>();
        uiHallNodeComponent.BtnPack.onClick.AddListener(() =>
        {
            UIKit.OpenUI<UIPackNodeComponent>();
        });

        uiHallNodeComponent.BtnEquip.onClick.AddListener(() =>
        {
            UIKit.OpenUI<UIEquipNodeComponent>();
        });
    }

    public override void Hide(ref EcsEntity ecsEntity)
    {
        base.Hide(ref ecsEntity);
        Debug.Log("我是大厅面板，我被隐藏了- OnHide");
    }

    public override void Freeze(ref EcsEntity ecsEntity)
    {
        base.Freeze(ref ecsEntity);
        Debug.Log("我是大厅面板，我被冻结了- OnFreeze");
    }

    public override void ReShow(ref EcsEntity ecsEntity)
    {
        base.ReShow(ref ecsEntity);
        Debug.Log("我是大厅面板，我又被显示了- ReShow");
    }
}