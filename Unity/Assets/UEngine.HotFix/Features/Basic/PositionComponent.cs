using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Unit,Event(EventTarget.Self)]
public sealed class PositionComponent : IComponent
{
    public Vector3 value;
}
