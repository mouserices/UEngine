using System.Collections.Generic;
using Entitas;
using UEngine.NP.FsmState;

[Unit]
public class StateComponent:IComponent
{
    public LinkedList<FsmStateBase> FsmStateBases;
}