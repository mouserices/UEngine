using System.Collections.Generic;
using Entitas;
using UEngine.NP.FsmState;

public class StateRemoveSystem : ReactiveSystem<UnitEntity>
{
    public StateRemoveSystem(Contexts contexts) : base(contexts.unit)
    {
    }

    protected override ICollector<UnitEntity> GetTrigger(IContext<UnitEntity> context) => context.CreateCollector(UnitMatcher.StateExit);


    protected override bool Filter(UnitEntity entity) => entity.hasStateExit;
    

    protected override void Execute(List<UnitEntity> entities)
    {
        foreach (UnitEntity entity in entities)
        {
            RemoveState(entity);
        }
    }
    private void RemoveState(UnitEntity ecsEntity)
    {
        var stateType = ecsEntity.stateExit.StateType;

        FsmStateBase fsmStateBaseToRemove = null;
        foreach (FsmStateBase runtimeFsmStateBase in ecsEntity.state.FsmStateBases)
        {
            if (runtimeFsmStateBase.StateType == stateType)
            {
                fsmStateBaseToRemove = runtimeFsmStateBase;
                break;
            }
        }

        bool isFirstState = false;
        if (ecsEntity.state.FsmStateBases.First != null)
        {
            isFirstState = ecsEntity.state.FsmStateBases.First.Value.StateType == stateType;
        }
        if (fsmStateBaseToRemove != null)
        {
            fsmStateBaseToRemove.OnRemove();
            ecsEntity.state.FsmStateBases.Remove(fsmStateBaseToRemove);
        }

        if (isFirstState)
        {
            ecsEntity.state.FsmStateBases.First?.Value.Link(ecsEntity);
            ecsEntity.state.FsmStateBases.First?.Value.OnEnter();
        }
        
        ecsEntity.RemoveStateExit();
    }
}