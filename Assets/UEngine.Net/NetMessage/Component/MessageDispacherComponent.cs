using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [NetMessage]
    [Cleanup(CleanupMode.DestroyEntity)]
    public class MessageDispacherComponent : IComponent
    {
        public byte[] Bytes;
    }
}