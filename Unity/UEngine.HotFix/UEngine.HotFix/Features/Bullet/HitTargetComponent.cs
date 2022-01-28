using System.Collections.Generic;
using Entitas;
[Unit]
public class HitTargetComponent : IComponent
{
    public List<HitTarget> Targets;
}