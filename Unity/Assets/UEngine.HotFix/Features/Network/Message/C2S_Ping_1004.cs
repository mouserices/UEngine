using ProtoBuf;

namespace UEngine.Net
{
    [ProtoContract]
    [MessageOpCode(1004)]
    [MessageResponse(typeof(S2C_Ping_1005))]
    public class C2S_Ping_1004 : IRequest
    {
        [ProtoMember(1)]
        public int RpcID { get; set; }

        [ProtoMember(2)]
        public long SendTimestamp;
    }
}