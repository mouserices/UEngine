using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Meta,Unique]
public class ServiceComponent : IComponent
{
    public Dictionary<Type, IService> RegisterServices;
}