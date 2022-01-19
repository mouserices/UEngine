using ProtoBuf;

namespace UEngine.Net
{
    [MessageOpCode(1007)]
    [ProtoContract]
    public class S2C_EnterRoom_1007 : IResponse
    {
        [ProtoMember(1)]
        public int RpcID { get; set; }

        [ProtoMember(2)]
        public bool Succeed;
    }
}