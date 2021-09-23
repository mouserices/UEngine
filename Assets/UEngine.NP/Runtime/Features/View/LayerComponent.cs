using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
[Event(EventTarget.Self)]
public class LayerComponent : IComponent
{
    public int Value;
}