using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UEngine.NP;
using UEngine.UI.Event;
using UEngine.UI.Runtime.System;
using UEngine.UI.System;
using UnityEngine;

public class ExampleUIGameSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EcsWorld _world = new EcsWorld();

        EcsSystems _systems = new EcsSystems(_world)
                .Add(new AttributeSearcherSystem())
                .Add(new NP_TreeDataSystem())
                
                .Add(new UIConfigSystem())
                .Add(new UILoadSystem())
                .Add(new UICloseSystem())
                .Add(new UIVisableSystem())
                .Add(new UIMaskSystem())
                
                .Add(new UILoginSystem())
                .Add(new UIHallSystem())
                .Add(new UIPlayerInfoSystem())
                .Add(new UIPackSystem())
                .Add(new UIPropSystem())
                .Add(new UIEquipSystem())
                .Add(new GameStartSystem())
                
                
                //event
                .OneFrame<RequestLoadUIEvent>()
                .OneFrame<RequestLoadAssetEvent>()
                .OneFrame<UIOpenEvent>()
                .OneFrame<UICloseEvent>()
                .OneFrame<UIFreezeEvent>()
                .OneFrame<UIHideEvent>()
                .OneFrame<UIShowEvent>()
                .OneFrame<UIMaskEvent>()
            ;
        Game.EcsSystems = _systems;
        Game.EcsWorld = _world;
        _systems.Init();
    }

    // Update is called once per frame
    void Update()
    {
        Game.EcsSystems?.Run();
    }
}
