using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [NetMessage]
    public class MessageSendComponent : IComponent
    {
        public int connectedID;
        public int OpCode;
        public IMessage Message;
    }
}