using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Telepathy;
using UnityEngine;

[Room,Cleanup(CleanupMode.DestroyEntity)]
public class AddPlayerMessageComponent : IComponent
{
    public int RoomID;
    public int ConnectionId;
    public int UnitId;
    public long PlayerID;
    public string Asset;
    public Vector3 Pos;
    public Vector3 Rot;
    public List<int> Skills;
    public CampType CampType;
}