using Leopotam.Ecs;
using UEngine.UI.Component;
using UEngine.UI.Event;
using UnityEngine;

[AutoInjectSystem]
public class UILoginSystem : UIBaseSystem<UILoginNodeComponent>
{
    private int clickNum = 0;
    public override void Open(ref EcsEntity ecsEntity)
    {
        base.Open(ref ecsEntity);
    
        var uiLoginNodeComponent = ecsEntity.Get<UILoginNodeComponent>();
        uiLoginNodeComponent.Button.onClick.AddListener(() =>
        {
            clickNum++;
            uiLoginNodeComponent.InputField.text = $"点击了{clickNum}";
            
            UIKit.CloseUI<UILoginNodeComponent>();
            
            UIKit.OpenUI<UIHallNodeComponent>();
            UIKit.OpenUI<UIPlayerInfoNodeComponent>();
        });
    }
}