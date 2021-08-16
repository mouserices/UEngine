using Leopotam.Ecs;
using UEngine.UI.Event;


public class GameStartSystem : IEcsInitSystem, IEcsRunSystem
{
    public void Init()
    {
        UIKit.OpenUI<UILoginNodeComponent>();
    }

    public void Run()
    {
    }
}