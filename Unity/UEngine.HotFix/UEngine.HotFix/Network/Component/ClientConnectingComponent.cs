using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [Network]
    [Unique]
    [Cleanup(CleanupMode.RemoveComponent)]
    public class ClientConnectingComponent : IComponent
    {
        public string IP;
        public int Port;
    }
}