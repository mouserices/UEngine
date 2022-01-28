using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unit]
public class NameComponent : IComponent
{
    [PrimaryEntityIndex]
    public string Value;
}