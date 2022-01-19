using Entitas;
using UnityEngine;


public class StandardMoveSystem : ITickSystem
{
    private Contexts _Contexts;
    private IGroup<UnitEntity> _MoveGroup;
    public StandardMoveSystem(Contexts contexts)
    {
        _Contexts = contexts;
        _MoveGroup = _Contexts.unit.GetGroup(UnitMatcher.Move);
    }
    public void Tick()
    {
        if (_MoveGroup.count > 0)
        {
            for (int i = 0; i < _MoveGroup.GetEntities().Length; i++)
            {
                var moveEntity = _MoveGroup.GetEntities()[i];
                if (moveEntity.hasMove && moveEntity.move.IsMoving)
                {
                    Vector3 moveDir = new Vector3(moveEntity.move.MoveDir.x, 0, moveEntity.move.MoveDir.y);
                    var moveMoveSpeed = moveEntity.position.value + moveDir *
                        moveEntity.move.MoveSpeed * (1 / 20f); //Time.deltaTime;  1 / 30f
                    
                    moveEntity.ReplacePosition(moveMoveSpeed);

                    var eulerAngles = Quaternion.LookRotation(moveDir).eulerAngles;
                    moveEntity.ReplaceRotation(eulerAngles);

                }
            }
        }
    }
}