using Entitas;
using Entitas.CodeGeneration.Attributes;
using UEngine.NP.FsmState;


[Game, Cleanup(CleanupMode.RemoveComponent)]
public class StateExitComponent : IComponent
{
    public StateType StateType;
}