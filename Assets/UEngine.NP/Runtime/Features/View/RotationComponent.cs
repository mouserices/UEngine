using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Event(EventTarget.Self)]
public class RotationComponent : IComponent
{
    public Vector3 Value;
}