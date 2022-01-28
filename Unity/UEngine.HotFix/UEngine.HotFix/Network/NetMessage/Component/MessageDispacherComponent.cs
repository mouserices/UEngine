using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [NetMessage]
    public class MessageDispacherComponent : IComponent
    {
        public int ConnectionId;
        public byte[] Bytes;
    }
}