using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Unit,Event(EventTarget.Self)]
public class ScaleComponent : IComponent
{
    public Vector3 Value;
}