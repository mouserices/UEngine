using System;
using System.Collections.Generic;
using System.Reflection;
using Leopotam.Ecs;
using UnityEngine;
using Object = System.Object;

public struct UIObjectComponent
{
    public GameObject UIRootCanvas;
    public Transform TransUIRoot;
    public Transform TransNormal;
    public Transform TransFixed;
    public Transform TransPopUp;
    public Transform TransMask;

    public Dictionary<Type, FieldInfo[]> FieldInfos;
    public Dictionary<string, Type> NodeNameToNodeType;
    public Dictionary<Type, Object> UINodeObjects;

    public Dictionary<string, UIConfig> UIConfigs;
    
    public Dictionary<string, EcsEntity> UIEntities;
    public Dictionary<string, EcsEntity> CurrentShowUIEntitys;
    public Stack<EcsEntity> StackEntities;
    public Dictionary<Type, EcsEntity> UITypeToEntities;
}