using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Cleanup(CleanupMode.RemoveComponent)]
public class BehaveTreeLoadComponent : IComponent
{
    public List<string> BehaveTreeNames;
}