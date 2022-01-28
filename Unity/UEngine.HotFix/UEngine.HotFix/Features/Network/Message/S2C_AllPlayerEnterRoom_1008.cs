using System.Collections.Generic;
using ProtoBuf;
using UEngine.Net;

[ProtoContract]
[MessageOpCode(1008)]
public class S2C_AllPlayerEnterRoom_1008 : ISimpleMessage
{
    [ProtoMember(1)]
    public int RpcID { get; set; }
    [ProtoMember(2)]
    public int RoomID;
    [ProtoMember(3)]
    public List<UnitData> Players;
}