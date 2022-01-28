using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unit,Game,Event(EventTarget.Self),Cleanup(CleanupMode.DestroyEntity)]
public sealed class DestroyedComponent : IComponent
{
}
