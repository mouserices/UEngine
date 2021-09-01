using System.Collections.Generic;
using Entitas;
using UEngine.NP.FsmState;

[Game]
public class StateComponent:IComponent
{
    public LinkedList<FsmStateBase> FsmStateBases;
}