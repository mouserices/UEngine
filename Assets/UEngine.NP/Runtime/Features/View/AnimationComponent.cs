using Entitas;
using Entitas.CodeGeneration.Attributes;


[Event(EventTarget.Self)]
public class AnimationComponent : IComponent
{
    public string AnimClipName;
}