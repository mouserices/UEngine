using System.Collections.Generic;
using Entitas;

[Unit]
public class NumericComponent : IComponent
{
    public Dictionary<NumericType, float> Numerics;
}