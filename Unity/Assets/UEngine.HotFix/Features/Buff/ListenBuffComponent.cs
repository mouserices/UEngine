using System;
using System.Collections.Generic;
using Entitas;
using UEngine.NP;

[Buff]
public class ListenBuffComponent : IComponent
{
    public Func<bool> BuffCondition;
    public List<VTD_Id> BuffEffects;
}