using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public class UnitComponent : IComponent
{
    [PrimaryEntityIndex]
    public long ID;
}