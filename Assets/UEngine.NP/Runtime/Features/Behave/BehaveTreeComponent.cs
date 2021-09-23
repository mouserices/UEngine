using System;
using System.Collections.Generic;
using Entitas;
using Entitas.VisualDebugging.Unity;
using NPBehave;

[Game,RemoteAgent,DontDrawComponent]
public class SkillContainerComponent : IComponent
{
    public List<Skill> Skills;
}