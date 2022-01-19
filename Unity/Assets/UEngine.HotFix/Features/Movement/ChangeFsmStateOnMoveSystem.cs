using Entitas;
using UEngine.NP.Features.FsmState;
using UEngine.NP.FsmState;

public class ChangeFsmStateOnMoveSystem : ITickSystem
{
    private Contexts _Contexts;
    private IGroup<UnitEntity> _MoveGroup;
    public ChangeFsmStateOnMoveSystem(Contexts contexts)
    {
        _Contexts = contexts;
        _MoveGroup = _Contexts.unit.GetGroup(UnitMatcher.AllOf(UnitMatcher.Move,UnitMatcher.State));
    }
    public void Tick()
    {
        if (_MoveGroup.count > 0)
        {
            for (int i = 0; i < _MoveGroup.GetEntities().Length; i++)
            {
                var moveEntity = _MoveGroup.GetEntities()[i];
                if (moveEntity.move.IsMoving && moveEntity.CheckState(StateType.IDLE))
                {
                    moveEntity.ReplaceStateEnter(new WalkStateParam(){AnimClipName = "Walk",StateType = StateType.Walk});
                }
                
                if (!moveEntity.move.IsMoving && moveEntity.CheckState(StateType.Walk))
                {
                    moveEntity.ReplaceStateEnter(new IdleStateParam(){AnimClipName = "Idle",StateType = StateType.IDLE});
                }
            }
        }
    }
}