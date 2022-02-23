using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [Network]
    [Unique]
    [Cleanup(CleanupMode.RemoveComponent)]
    public class ServerListeningComponent : IComponent
    {
        public int Port;
    }
}