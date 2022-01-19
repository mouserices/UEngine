using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game,Unique]
public class MainPlayerDataComponent : IComponent
{
    public long PlayerID;
    public int RoomID;
}