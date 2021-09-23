using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
[RemoteAgent]
public class UnitComponent : IComponent
{
    [PrimaryEntityIndex]
    public long ID;
}