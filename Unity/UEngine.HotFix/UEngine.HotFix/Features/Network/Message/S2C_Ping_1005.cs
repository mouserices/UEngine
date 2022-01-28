using ProtoBuf;

namespace UEngine.Net
{
    [MessageOpCode(1005)]
    [ProtoContract]
    public class S2C_Ping_1005 : IResponse
    {
        [ProtoMember(1)]
        public int RpcID { get; set; }

        [ProtoMember(2)]
        public long SendTimestamp;
        [ProtoMember(3)]
        public long ServerTotalTicks;
    }
}