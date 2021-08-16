using Leopotam.Ecs;
using UEngine.UI.Component;
using UnityEngine;

namespace UEngine.UI.System
{
    [AutoInjectSystem]
    public class UIPlayerInfoSystem : UIBaseSystem<UIPlayerInfoNodeComponent>
    {
        public override void Open(ref EcsEntity ecsEntity)
        {
            base.Open(ref ecsEntity);
            Debug.Log("我是玩家信息面板，我第一次被打开- OnOpen");
            
        }
        public override void Hide(ref EcsEntity ecsEntity)
        {
            base.Hide(ref ecsEntity);
            Debug.Log("我是玩家信息面板，我被隐藏了- OnHide");
        }

        public override void Freeze(ref EcsEntity ecsEntity)
        {
            base.Freeze(ref ecsEntity);
            Debug.Log("我是玩家信息面板，我被冻结了- OnFreeze");
        }

        public override void ReShow(ref EcsEntity ecsEntity)
        {
            base.ReShow(ref ecsEntity);
            Debug.Log("我是玩家信息面板，我又被显示了- ReShow");
        }
    }
}