using ProtoBuf;

namespace UEngine.Net
{
    [MessageOpCode(1002)]
    [ProtoContract]
    public class S2C_Login : IResponse
    {
        [ProtoMember(1)]
        public int RpcID { get; set; }
        [ProtoMember(2)]
        public string UserName;

        [ProtoMember(3)] public string Password;
        [ProtoMember(4)] public long PlayerID;
    }
}