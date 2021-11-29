using Entitas;
using UnityEngine;


public class StandardMoveSystem : IExecuteSystem
{
    private Contexts _Contexts;
    private IGroup<GameEntity> _MoveGroup;
    public StandardMoveSystem(Contexts contexts, Services services)
    {
        _Contexts = contexts;
        _MoveGroup = _Contexts.game.GetGroup(GameMatcher.Move);
    }

    public void Execute()
    {
        if (_MoveGroup.count > 0)
        {
            for (int i = 0; i < _MoveGroup.GetEntities().Length; i++)
            {
                var moveEntity = _MoveGroup.GetEntities()[i];
                if (moveEntity.hasMove && moveEntity.move.IsMoving)
                {
                    Vector3 moveDir = new Vector3(moveEntity.move.MoveDir.x, 0, moveEntity.move.MoveDir.y);
                    var moveMoveSpeed = moveEntity.position.value +  moveDir*
                        moveEntity.move.MoveSpeed * Time.deltaTime;
                    
                    moveEntity.ReplacePosition(moveMoveSpeed);

                    var eulerAngles = Quaternion.LookRotation(moveDir).eulerAngles;
                    moveEntity.ReplaceRotation(eulerAngles);
                }
            }
        }
    }
}