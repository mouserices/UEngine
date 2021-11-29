using ProtoBuf;

namespace UEngine.Net
{
    [ProtoContract]
    public class C2S_Login : IMessage
    {
        [ProtoMember(1)]
        public string UserName;
        [ProtoMember(2)]
        public string Password;
    }
}