using System;
using System.Collections.Generic;
using System.Reflection;
using Leopotam.Ecs;
using UEngine.UI.Component;


public class AttributeSearcherSystem : IEcsInitSystem
{
    public void Init()
    {
        Game.MainEntity = Game.EcsWorld.NewEntity();

        ref var attributeComponent = ref Game.MainEntity.Get<AttributeComponent>();
        attributeComponent.AutoInjectSystems = new List<Type>();

        var types = this.GetType().Assembly.GetTypes();
        ref var _UINodeInjectComponent = ref Game.MainEntity.Get<UIObjectComponent>();
        _UINodeInjectComponent.NodeNameToNodeType = new Dictionary<string, Type>();
        _UINodeInjectComponent.FieldInfos = new Dictionary<Type, FieldInfo[]>();
        
        foreach (Type type in types)
        {
            var customAttributes = type.GetCustomAttributes(typeof(UINodeAttribute), false);
            if (customAttributes.Length > 0)
            {
                var fieldInfos = type.GetFields();
                RegisterFieldInfo(ref _UINodeInjectComponent, type, fieldInfos);
            }

            var attributes = type.GetCustomAttributes(typeof(AutoInjectSystemAttribute), false);
            if (attributes.Length > 0)
            {
                attributeComponent.AutoInjectSystems.Add(type);
            }
        }
    }

    private void RegisterFieldInfo(ref UIObjectComponent _UINodeInjectComponent, Type type, FieldInfo[] fieldInfos)
    {
        if (!_UINodeInjectComponent.FieldInfos.ContainsKey(type))
        {
            _UINodeInjectComponent.FieldInfos.Add(type, fieldInfos);
        }
        
        if (!_UINodeInjectComponent.NodeNameToNodeType.ContainsKey(type.Name))
        {
            _UINodeInjectComponent.NodeNameToNodeType.Add(type.Name, type);
        }
    }
}