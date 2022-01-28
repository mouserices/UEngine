using Entitas;
using Entitas.CodeGeneration.Attributes;
using UEngine.NP.FsmState;


[Unit, Cleanup(CleanupMode.RemoveComponent)]
public class StateExitComponent : IComponent
{
    public StateType StateType;
}