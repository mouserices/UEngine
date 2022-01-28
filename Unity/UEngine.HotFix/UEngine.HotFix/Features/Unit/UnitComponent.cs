using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unit]
public class UnitComponent : IComponent
{
    [PrimaryEntityIndex]
    public long ID;
}