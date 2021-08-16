using System.Security.Cryptography.X509Certificates;
using Leopotam.Ecs;
using UEngine.UI.Component;
using UnityEngine;


[AutoInjectSystem]
public class UIPackSystem : UIBaseSystem<UIPackNodeComponent>
{
    public override void Open(ref EcsEntity ecsEntity)
    {
        base.Open(ref ecsEntity);
        Debug.Log("我是背包面板，我第一次被打开- OnOpen");
        ref var uiPackNodeComponent = ref ecsEntity.Get<UIPackNodeComponent>();
        uiPackNodeComponent.Title.text = "我是背包面板";

        uiPackNodeComponent.BtnClose.onClick.AddListener(() => { UIKit.CloseUI<UIPackNodeComponent>(); });

        uiPackNodeComponent.BtnInfo.onClick.AddListener(() => { UIKit.OpenUI<UIPropInfoNodeComponent>(); });
    }

    public override void Hide(ref EcsEntity ecsEntity)
    {
        base.Hide(ref ecsEntity);
        Debug.Log("我是背包面板，我被隐藏了- OnHide");
    }

    public override void Freeze(ref EcsEntity ecsEntity)
    {
        base.Freeze(ref ecsEntity);
        Debug.Log("我是背包面板，我被冻结了- OnFreeze");
    }

    public override void ReShow(ref EcsEntity ecsEntity)
    {
        base.ReShow(ref ecsEntity);
        Debug.Log("我是背包面板，我又被显示了- ReShow");
    }
}