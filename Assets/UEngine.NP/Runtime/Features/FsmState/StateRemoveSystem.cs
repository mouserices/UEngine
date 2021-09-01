using System.Collections.Generic;
using Entitas;
using UEngine.NP.FsmState;

public class StateRemoveSystem : ReactiveSystem<GameEntity>
{
    public StateRemoveSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) => context.CreateCollector(GameMatcher.StateExit);


    protected override bool Filter(GameEntity entity) => entity.hasStateExit;
    

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity entity in entities)
        {
            RemoveState(entity);
        }
    }
    private void RemoveState(GameEntity ecsEntity)
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
    }
}