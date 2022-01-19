using System.Collections.Generic;
using Entitas;
[Unit]
public class NumericModifierComponent : IComponent
{
    public Dictionary<NumericType, List<BaseModifier>> Modifiers;
}