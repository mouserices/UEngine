using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public class NameComponent : IComponent
{
    [PrimaryEntityIndex]
    public string Value;
}