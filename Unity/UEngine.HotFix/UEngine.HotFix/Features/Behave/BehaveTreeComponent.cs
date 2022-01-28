using System.Collections.Generic;
using Entitas;
using Entitas.VisualDebugging.Unity;

[Unit,DontDrawComponent]
public class SkillContainerComponent : IComponent
{
    public List<Skill> Skills;
}