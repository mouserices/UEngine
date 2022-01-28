using UEngine.NP.FsmState;
using UnityEngine;

public class PatrolState : FsmStateBase
{
    public override StateType GetConflictStates()
    {
        return StateType.Patrol;
    }

    public override void OnEnter()
    {
        base.OnEnter();

#if CLIENT
        var gameEntity = entity as UnitEntity;
        gameEntity.ReplaceAnimation(this.StateParam.AnimClipName,1f,null);
        gameEntity.AddPatrol(new Vector3(250f,0,270f),8,0.05f);
#endif
    }
}