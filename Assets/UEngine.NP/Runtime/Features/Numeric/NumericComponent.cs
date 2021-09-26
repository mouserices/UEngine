using System.Collections.Generic;
using Entitas;

[Game]
public class NumericComponent : IComponent
{
    public Dictionary<NumericType, float> Numerics;
}