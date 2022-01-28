using ProtoBuf;
namespace UEngine.Net
{
    [ProtoContract]
    [MessageOpCode(1001)]
    [MessageResponse(typeof(S2C_Login))]
    public class C2S_Login : IRequest
    {
        [ProtoMember(1)]
        public string UserName;

        [ProtoMember(2)] public string Password;
        [ProtoMember(3)] public int RpcID { get; set; }
    }
}