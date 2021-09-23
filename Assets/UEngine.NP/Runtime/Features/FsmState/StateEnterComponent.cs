using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game,Cleanup(CleanupMode.RemoveComponent)]
public class StateEnterComponent : IComponent
{
    public StateParam StateParam;
}