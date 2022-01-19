using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unit]
public class ConnectionIDComponent : IComponent
{
    [PrimaryEntityIndex]
    public int ID;
}