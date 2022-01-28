using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
[Unit]
[Event(EventTarget.Self)]
public class ParentComponent : IComponent
{
    public Transform Value;
}