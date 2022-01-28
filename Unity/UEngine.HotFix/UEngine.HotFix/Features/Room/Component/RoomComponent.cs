using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Room]
public class RoomComponent : IComponent
{
    [PrimaryEntityIndex]
    public int RoomID;

    public Dictionary<long, UnitEntity> Players;
}