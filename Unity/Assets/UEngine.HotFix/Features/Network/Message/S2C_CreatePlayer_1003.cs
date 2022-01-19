using System.Collections.Generic;
using ProtoBuf;

namespace UEngine.Net
{
    [ProtoContract]
    public class UnitData
    {
        [ProtoMember(1)] public int ID;
        [ProtoMember(2)] public string Asset;
        [ProtoMember(3)] public float PosX;
        [ProtoMember(4)] public float PosY;
        [ProtoMember(5)] public float PosZ;
        [ProtoMember(6)] public float RotX;
        [ProtoMember(7)] public float RotY;
        [ProtoMember(8)] public float RotZ;
        [ProtoMember(9)] public List<int> Skills;
        [ProtoMember(10)] public int CampType;
        [ProtoMember(11)] public long PlayerID;
    }

    [MessageOpCode(1003)]
    [ProtoContract]
    public class S2C_CreatePlayer_1003 : ISimpleMessage
    {
        [ProtoMember(1)] public UnitData Unit;
        [ProtoMember(2)] public int RpcID { get; set; }
    }
}