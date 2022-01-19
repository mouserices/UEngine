using System;
using System.Collections.Generic;
using UEngine.NP.FsmState;

public class ComboState : FsmStateBase
{
    public override StateType GetConflictStates()
    {
        return StateType.NONE;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        playAnimFromStack();
    }

    private void playAnimFromStack()
    {
        var gameEntity = entity as UnitEntity;

        var comboStateParam = StateParam as ComboStateParam;
        if (comboStateParam.StateParams.Count > 0)
        {
            var stateParam = comboStateParam.StateParams.Pop();

#if CLIENT
          //play anim
            gameEntity.ReplaceAnimation(stateParam.AnimClipName, stateParam.Speed,
                () =>
                {
                    playAnimFromStack();
                });  
#endif
            
        }
        else
        {
            gameEntity.ReplaceStateExit(StateType.Combo);
        }
    }

    public override void OnExist()
    {
        
    }

    public void AddComboAnimToStack(StateParam stateParam)
    {
        var comboStateParam = StateParam as ComboStateParam;
        comboStateParam.StateParams.Push(stateParam);
    }
}