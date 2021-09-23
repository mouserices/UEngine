using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Event(EventTarget.Self)]
public class ScaleComponent : IComponent
{
    public Vector3 Value;
}