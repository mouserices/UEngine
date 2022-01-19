using System;
using System.Collections.Generic;
using Entitas;
using UEngine.NP.FsmState;

public class StateChangeSystem : ReactiveSystem<UnitEntity>
{
    public StateChangeSystem(Contexts contexts) : base(contexts.unit)
    {
    }

    protected override ICollector<UnitEntity> GetTrigger(IContext<UnitEntity> context)
    {
        return context.CreateCollector(UnitMatcher.StateEnter);
    }

    protected override bool Filter(UnitEntity entity)
    {
#if CLIENT
        return entity.hasStateEnter && entity.hasState && entity.hasView;
#endif
        return entity.hasStateEnter && entity.hasState;
    }


    protected override void Execute(List<UnitEntity> entities)
    {
        foreach (UnitEntity entity in entities)
        {
            ChangeState(entity);
        }
    }

    private void ChangeState(UnitEntity entity)
    {
        var stateType = entity.stateEnter.StateParam.StateType;

        var firstState = entity.state.FsmStateBases.First;

        if (firstState != null && firstState.Value.StateType == stateType)
        {
            return;
        }

        //状态冲突处理
        FsmStateBase stateToEnter = CreateState(stateType);

        if (firstState != null && !stateToEnter.TryEnter(firstState.Value.StateType))
        {
            return;
        }

        //寻找相同的优先级
        LinkedListNode<FsmStateBase> current = firstState;
        while (current != null)
        {
            if (stateToEnter.Priority >= current.Value.Priority)
            {
                break;
            }

            current = current.Next;
        }


        bool isContainerSelf = false;
        foreach (FsmStateBase fsmStateBase in entity.state.FsmStateBases)
        {
            if (fsmStateBase.StateType == stateType)
            {
                isContainerSelf = true;
                break;
            }
        }

        //如果自身包含了目标状态，则把目标状态放在相同优先级的前面
        if (isContainerSelf)
        {
            entity.state.FsmStateBases.Remove(stateToEnter);
            entity.state.FsmStateBases.AddBefore(current, stateToEnter);
        }
        else
        {
            if (current == null)
            {
                entity.state.FsmStateBases.AddLast(stateToEnter);
            }
            else
            {
                entity.state.FsmStateBases.AddBefore(current, stateToEnter);
            }
        }


        var b = entity.state.FsmStateBases.First.Value.StateType == stateType;
        if (b)
        {
            firstState?.Value.OnExist();
            stateToEnter.Link(entity);
            stateToEnter.StateParam = entity.stateEnter.StateParam;
            stateToEnter.OnEnter();
        }
        
        entity.RemoveStateEnter();
    }

    private FsmStateBase CreateState(StateType stateType)
    {
        FsmStateBase state = null;
        switch (stateType)
        {
            case StateType.NONE:
                break;
            case StateType.IDLE:
                state = new IdleState();
                break;
            case StateType.Walk:
                state = new WalkState();
                break;
            case StateType.Run:
                state = new RunState();
                break;
            case StateType.Attack:
                state = new AttackState();
                break;
            case StateType.Patrol:
                state = new PatrolState();
                break;
            case StateType.Combo:
                state = new ComboState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null);
        }

        state.StateType = stateType;
        return state;
    }
}