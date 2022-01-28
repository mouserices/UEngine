using ProtoBuf;

namespace UEngine.Net
{
    [ProtoContract]
    [MessageOpCode(1006)]
    [MessageResponse(typeof(S2C_EnterRoom_1007))]
    public class C2S_EnterRoom_1006 : IRequest
    {
        [ProtoMember(1)]
        public int RpcID { get; set; }
        [ProtoMember(2)]
        public int RoomID { get; set; }
    }
}