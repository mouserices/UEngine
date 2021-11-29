using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace UEngine.Net
{
    [Network]
    [Unique]
    public class ServerListeningComponent : IComponent
    {
        public int Port;
    }
}