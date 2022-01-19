using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [NetMessage]
    [Cleanup(CleanupMode.DestroyEntity)]
    public class MessageSendComponent : IComponent
    {
        public int OpCode;
        public IMessage Message;
    }
}