using System.Collections.Generic;
using Entitas;
[Game]
public class NumericModifierComponent : IComponent
{
    public Dictionary<NumericType, List<BaseModifier>> Modifiers;
}