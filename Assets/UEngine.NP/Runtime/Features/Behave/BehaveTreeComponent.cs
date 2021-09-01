using System;
using System.Collections.Generic;
using Entitas;
using Entitas.VisualDebugging.Unity;
using NPBehave;

[Game,DontDrawComponent]
public class BehaveTreeComponent : IComponent
{
    public List<Root> BehaveTreeRoots;
}