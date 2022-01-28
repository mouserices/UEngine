using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unit]
[Event(EventTarget.Self)]
public class LayerComponent : IComponent
{
    public int Value;
}