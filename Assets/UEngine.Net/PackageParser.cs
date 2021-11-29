using ProtoBuf;
namespace UEngine.Net
{
    [ProtoContract]
    public class Package
    {
        public int OpCode;
        public byte[] MsgBody;
    }

    public class PackageParser
    {
        
    }
}