using Entitas;
using Entitas.CodeGeneration.Attributes;

[Room,Cleanup(CleanupMode.DestroyEntity)]
public class RemovePlayerMessageComponent : IComponent
{
    public long UnitID;
    public int RoomID;
}