using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unit, Cleanup(CleanupMode.RemoveComponent)]
public class BehaveTreeLoadComponent : IComponent
{
    public List<int> SkillIDs;
}