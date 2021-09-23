using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Event(EventTarget.Self)]
public class ParentComponent : IComponent
{
    public Transform Value;
}