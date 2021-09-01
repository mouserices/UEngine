using System;
using System.Collections.Generic;
using Entitas;
using UEngine.NP.FsmState;
using UnityEngine;

public class StateChangeSystem : ReactiveSystem<GameEntity>
{
    public StateChangeSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.StateEnter);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasStateEnter && entity.hasState && entity.hasView;
    }


    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity entity in entities)
        {
            ChangeState(entity);
        }
    }

    private void ChangeState(GameEntity entity)
    {
        var stateType = entity.stateEnter.StateParam.GetStateType();

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
            default:
                throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null);
        }

        state.StateType = stateType;
        return state;
    }
}