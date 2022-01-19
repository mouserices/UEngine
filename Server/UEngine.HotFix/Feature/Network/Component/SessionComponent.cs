using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Network,Unique]
public class SessionComponent : IComponent
{
    public Dictionary<int, Session> ConnectedIDToSessions;
}