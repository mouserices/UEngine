using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unit]
[Event(EventTarget.Self)]
public class AnimationComponent : IComponent
{
    public string AnimClipName;
    public float Speed;
    public Action OnEnd;
}